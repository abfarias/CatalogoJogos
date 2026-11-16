using CatalogoJogos.Models;

namespace CatalogoJogos.Interfaces
{
    public interface IJogoRepository : IRepository<Jogo>
    {
        Task<List<Jogo>> BuscarAsync(string? termo, string? categoria, bool somenteAtivos);
        Task<List<string>> ListarCategoriasAsync();
    }
}
