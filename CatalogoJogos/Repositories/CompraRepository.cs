using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Repositories
{
    public class CompraRepository : EfRepository<Compra>, ICompraRepository
    {
        public CompraRepository(CatalogoJogosDbContext contexto) : base(contexto) { }

        public Task<bool> ExisteAsync(int idJogador, int idJogo)
        {
            return Conjunto.AnyAsync(c => c.IdJogador == idJogador && c.IdJogo == idJogo);
        }

        public Task<List<Compra>> ListarPorJogadorAsync(int idJogador)
        {
            return Conjunto
                .Include(c => c.Jogo)
                .Where(c => c.IdJogador == idJogador)
                .OrderByDescending(c => c.DataAquisicao)
                .ToListAsync();
        }
    }
}
