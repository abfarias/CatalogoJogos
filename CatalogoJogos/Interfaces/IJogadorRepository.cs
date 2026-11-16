using CatalogoJogos.Models;

namespace CatalogoJogos.Interfaces
{
    public interface IJogadorRepository : IRepository<Jogador>
    {
        Task<Jogador?> ObterPorEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
        Task<bool> ExisteApelidoAsync(string apelido, int? ignorarIdJogador = null);
    }
}
