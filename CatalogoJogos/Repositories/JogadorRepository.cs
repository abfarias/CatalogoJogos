using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Repositories
{
    public class JogadorRepository : EfRepository<Jogador>, IJogadorRepository
    {
        public JogadorRepository(CatalogoJogosDbContext contexto) : base(contexto) { }

        public Task<Jogador?> ObterPorEmailAsync(string email)
        {
            return Conjunto.FirstOrDefaultAsync(j => j.Email == email);
        }

        public Task<bool> ExisteEmailAsync(string email)
        {
            return Conjunto.AnyAsync(j => j.Email == email);
        }

        public Task<bool> ExisteApelidoAsync(string apelido, int? ignorarIdJogador = null)
        {
            return Conjunto.AnyAsync(j => j.Apelido == apelido
                && (ignorarIdJogador == null || j.IdJogador != ignorarIdJogador));
        }
    }
}
