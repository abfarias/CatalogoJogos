using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoJogos.Repositories
{
    public class AdministradorRepository : EfRepository<Administrador>, IAdministradorRepository
    {
        public AdministradorRepository(CatalogoJogosDbContext contexto) : base(contexto) { }

        public Task<Administrador?> ObterPorEmailAsync(string email)
        {
            return Conjunto.FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
