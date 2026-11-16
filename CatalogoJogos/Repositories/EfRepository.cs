using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> Conjunto;

        public EfRepository(CatalogoJogosDbContext contexto)
        {
            Conjunto = contexto.Set<T>();
        }

        public Task<List<T>> ListarTodosAsync()
        {
            return Conjunto.ToListAsync();
        }

        public async Task<T?> ObterPorIdAsync(int id)
        {
            return await Conjunto.FindAsync(id);
        }

        public async Task AdicionarAsync(T entidade)
        {
            await Conjunto.AddAsync(entidade);
        }
    }
}
