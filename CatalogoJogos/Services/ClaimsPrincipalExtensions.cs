using System.Security.Claims;

namespace CatalogoJogos.Services
{
    public static class ClaimsPrincipalExtensions
    {
        // Id do jogador ou do administrador autenticado (claim NameIdentifier); nulo para visitantes.
        public static int? ObterIdUsuario(this ClaimsPrincipal usuario)
        {
            var valor = usuario.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(valor, out var id) ? id : null;
        }

        public static bool EhJogador(this ClaimsPrincipal usuario) => usuario.IsInRole(Perfis.Jogador);

        public static bool EhAdministrador(this ClaimsPrincipal usuario) => usuario.IsInRole(Perfis.Administrador);
    }
}
