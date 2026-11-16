using CatalogoJogos.Models;

namespace CatalogoJogos.Interfaces
{
    public interface ICompraRepository : IRepository<Compra>
    {
        Task<bool> ExisteAsync(int idJogador, int idJogo);
        Task<List<Compra>> ListarPorJogadorAsync(int idJogador);
    }
}
