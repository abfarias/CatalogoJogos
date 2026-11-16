namespace CatalogoJogos.Models
{
    public class Avaliacao
    {
        public int IdAvaliacao { get; set; }
        public int Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int IdJogador { get; set; }
        public int IdJogo { get; set; }

        public Jogador? Jogador { get; set; }
        public Jogo? Jogo { get; set; }
    }
}
