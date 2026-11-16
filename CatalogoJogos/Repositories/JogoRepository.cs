using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Repositories
{
    public class JogoRepository : EfRepository<Jogo>, IJogoRepository
    {
        public JogoRepository(CatalogoJogosDbContext contexto) : base(contexto) { }

        public Task<List<Jogo>> BuscarAsync(string? termo, string? categoria, bool somenteAtivos)
        {
            IQueryable<Jogo> consulta = Conjunto;

            if (somenteAtivos)
                consulta = consulta.Where(j => j.Ativo);

            if (!string.IsNullOrWhiteSpace(termo))
            {
                var termoLimpo = termo.Trim();
                consulta = consulta.Where(j => j.Titulo.Contains(termoLimpo));
            }

            if (!string.IsNullOrWhiteSpace(categoria))
                consulta = consulta.Where(j => j.Categoria == categoria);

            return consulta.OrderBy(j => j.Titulo).ToListAsync();
        }

        public Task<List<string>> ListarCategoriasAsync()
        {
            return Conjunto
                .Where(j => j.Ativo)
                .Select(j => j.Categoria)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}
