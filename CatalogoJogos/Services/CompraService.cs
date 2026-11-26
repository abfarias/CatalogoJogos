using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Services
{
    public class CompraService
    {
        public const string MensagemSaldoInsuficiente = "Saldo insuficiente.";

        private readonly IJogoRepository _jogoRepository;
        private readonly IJogadorRepository _jogadorRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public CompraService(
            IJogoRepository jogoRepository,
            IJogadorRepository jogadorRepository,
            ICompraRepository compraRepository,
            IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _jogoRepository = jogoRepository;
            _jogadorRepository = jogadorRepository;
            _compraRepository = compraRepository;
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }

        public async Task<ResultadoOperacao<Compra>> ComprarAsync(int idJogador, int idJogo)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            if (jogo is null || !jogo.Ativo)
                return ResultadoOperacao<Compra>.Falha("Jogo indisponível.");

            var jogador = await _jogadorRepository.ObterPorIdAsync(idJogador);
            if (jogador is null)
                return ResultadoOperacao<Compra>.Falha("Jogador não encontrado.");

            if (await _compraRepository.ExisteAsync(idJogador, idJogo))
                return ResultadoOperacao<Compra>.Falha("Você já possui este jogo.");

            if (!jogador.PodeComprar(jogo.Preco))
                return ResultadoOperacao<Compra>.Falha(MensagemSaldoInsuficiente);

            jogador.Debitar(jogo.Preco);
            var compra = Compra.Criar(jogador, jogo);
            await _compraRepository.AdicionarAsync(compra);

            // Débito e compra são gravados juntos: o SaveChanges do EF roda em uma única transação.
            try
            {
                await _unidadeDeTrabalho.SalvarAlteracoesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Outra operação alterou o saldo entre a leitura e a gravação; nada foi gravado.
                return ResultadoOperacao<Compra>.Falha("O saldo foi alterado por outra operação. Tente novamente.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
            {
                // Índice único (IdJogador, IdJogo): a mesma compra chegou duas vezes ao mesmo tempo.
                return ResultadoOperacao<Compra>.Falha("Você já possui este jogo.");
            }

            return ResultadoOperacao<Compra>.Ok(compra, $"Jogo \"{jogo.Titulo}\" adicionado à sua biblioteca.");
        }

        public Task<List<Compra>> ListarBibliotecaAsync(int idJogador)
        {
            return _compraRepository.ListarPorJogadorAsync(idJogador);
        }
    }
}
