using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Services
{
    public class AvaliacaoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public AvaliacaoService(
            IJogoRepository jogoRepository,
            ICompraRepository compraRepository,
            IAvaliacaoRepository avaliacaoRepository,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _jogoRepository = jogoRepository;
            _compraRepository = compraRepository;
            _avaliacaoRepository = avaliacaoRepository;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        public async Task<ResultadoOperacao<Avaliacao>> AvaliarAsync(int idJogador, int idJogo, int nota, string? comentario)
        {
            if (!Avaliacao.NotaValida(nota))
                return ResultadoOperacao<Avaliacao>.Falha("A nota deve ser de 1 a 5.");

            if (comentario?.Trim().Length > Avaliacao.TamanhoMaximoComentario)
                return ResultadoOperacao<Avaliacao>.Falha("O comentário deve ter no máximo 500 caracteres.");

            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            if (jogo is null || !jogo.Ativo)
                return ResultadoOperacao<Avaliacao>.Falha("Jogo não encontrado.");

            if (!await _compraRepository.ExisteAsync(idJogador, idJogo))
                return ResultadoOperacao<Avaliacao>.Falha("Só quem possui o jogo pode avaliá-lo.");

            // Uma avaliação por jogador e jogo: se já existe, é substituída.
            var avaliacao = await _avaliacaoRepository.ObterAsync(idJogador, idJogo);
            var mensagem = "Avaliação atualizada.";
            if (avaliacao is null)
            {
                avaliacao = new Avaliacao { IdJogador = idJogador, IdJogo = idJogo };
                await _avaliacaoRepository.AdicionarAsync(avaliacao);
                mensagem = "Avaliação registrada.";
            }
            avaliacao.Atualizar(nota, comentario);

            try
            {
                await _unidadeDeTrabalho.SalvarAlteracoesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
            {
                // Índice único (IdJogador, IdJogo): a primeira avaliação chegou duas vezes ao mesmo tempo.
                return ResultadoOperacao<Avaliacao>.Falha("Sua avaliação já foi registrada. Atualize a página e tente novamente.");
            }

            return ResultadoOperacao<Avaliacao>.Ok(avaliacao, mensagem);
        }
    }
}
