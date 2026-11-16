using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;

namespace CatalogoJogos.Services
{
    public class CatalogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public CatalogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public Task<List<Jogo>> BuscarAsync(string? termo, string? categoria)
        {
            return _jogoRepository.BuscarAsync(termo, categoria, somenteAtivos: true);
        }

        public Task<List<string>> ListarCategoriasAsync()
        {
            return _jogoRepository.ListarCategoriasAsync();
        }
    }
}
