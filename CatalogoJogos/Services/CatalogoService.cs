using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;

namespace CatalogoJogos.Services
{
    public class CatalogoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly ICompraRepository _compraRepository;

        public CatalogoService(
            IJogoRepository jogoRepository,
            IAvaliacaoRepository avaliacaoRepository,
            ICompraRepository compraRepository)
        {
            _jogoRepository = jogoRepository;
            _avaliacaoRepository = avaliacaoRepository;
            _compraRepository = compraRepository;
        }

        public Task<List<Jogo>> BuscarAsync(string? termo, string? categoria)
        {
            return _jogoRepository.BuscarAsync(termo, categoria, somenteAtivos: true);
        }

        public Task<List<string>> ListarCategoriasAsync()
        {
            return _jogoRepository.ListarCategoriasAsync();
        }

        // Nulo quando o jogo não existe ou está desativado ("Jogo não encontrado").
        public async Task<DetalhesJogo?> ObterDetalhesAsync(int idJogo, int? idJogador)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(idJogo);
            if (jogo is null || !jogo.Ativo)
                return null;

            var possui = idJogador is not null
                && await _compraRepository.ExisteAsync(idJogador.Value, idJogo);

            return new DetalhesJogo
            {
                Jogo = jogo,
                NotaMedia = await _avaliacaoRepository.ObterMediaAsync(idJogo),
                Avaliacoes = await _avaliacaoRepository.ListarPorJogoAsync(idJogo),
                Possui = possui
            };
        }
    }
}
