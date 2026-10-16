namespace CatalogoJogos.Models
{
    public class Avaliacao
    {
        public int IdAvaliacao { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public int IdJogador { get; set; }
        public int IdJogo { get; set; }
    }
}
