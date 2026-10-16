namespace CatalogoJogos.Models
{
    public class Jogador
    {
        public int IdJogador { get; set; }
        public string Apelido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string FotoPerfil { get; set; } = string.Empty;
        public decimal SaldoCarteira { get; set; }
    }
}
