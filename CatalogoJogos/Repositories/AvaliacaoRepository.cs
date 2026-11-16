using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Repositories
{
    public class AvaliacaoRepository : EfRepository<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(CatalogoJogosDbContext contexto) : base(contexto) { }

        public Task<Avaliacao?> ObterAsync(int idJogador, int idJogo)
        {
            return Conjunto.FirstOrDefaultAsync(a => a.IdJogador == idJogador && a.IdJogo == idJogo);
        }

        public Task<List<Avaliacao>> ListarPorJogoAsync(int idJogo)
        {
            return Conjunto
                .Include(a => a.Jogador)
                .Where(a => a.IdJogo == idJogo)
                .OrderByDescending(a => a.DataAvaliacao)
                .ToListAsync();
        }

        public Task<double?> ObterMediaAsync(int idJogo)
        {
            return Conjunto
                .Where(a => a.IdJogo == idJogo)
                .AverageAsync(a => (double?)a.Nota);
        }
    }
}
