using CatalogoJogos.Models;

namespace CatalogoJogos.Interfaces
{
    public interface IAvaliacaoRepository : IRepository<Avaliacao>
    {
        Task<Avaliacao?> ObterAsync(int idJogador, int idJogo);
        Task<List<Avaliacao>> ListarPorJogoAsync(int idJogo);
        Task<double?> ObterMediaAsync(int idJogo);
    }
}
