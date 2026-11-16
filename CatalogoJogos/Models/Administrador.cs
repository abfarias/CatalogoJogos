namespace CatalogoJogos.Models
{
    public class Administrador
    {
        public int IdAdmin { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;

        public List<Jogo> JogosCadastrados { get; set; } = new();
    }
}
