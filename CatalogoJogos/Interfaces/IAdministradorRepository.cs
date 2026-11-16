using CatalogoJogos.Models;

namespace CatalogoJogos.Interfaces
{
    public interface IAdministradorRepository : IRepository<Administrador>
    {
        Task<Administrador?> ObterPorEmailAsync(string email);
    }
}
