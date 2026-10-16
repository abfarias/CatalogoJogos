namespace CatalogoJogos.Models
{
    public class Administradorcs
    {
        public int IdAdmin { get; set; }
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
    }
}
