namespace CatalogoJogos.Models
{
    public class Avaliacao
    {
        public const int NotaMinima = 1;
        public const int NotaMaxima = 5;
        public const int TamanhoMaximoComentario = 500;

        public int IdAvaliacao { get; set; }
        public int Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int IdJogador { get; set; }
        public int IdJogo { get; set; }

        public Jogador? Jogador { get; set; }
        public Jogo? Jogo { get; set; }

        public static bool NotaValida(int nota)
        {
            return nota >= NotaMinima && nota <= NotaMaxima;
        }

        // Substitui a avaliação anterior do jogador; a data passa a ser a da última alteração.
        public void Atualizar(int nota, string? comentario)
        {
            if (!NotaValida(nota))
                throw new ArgumentOutOfRangeException(nameof(nota), "A nota deve ser de 1 a 5.");

            Nota = nota;
            Comentario = string.IsNullOrWhiteSpace(comentario) ? null : comentario.Trim();
            DataAvaliacao = DateTime.Now;
        }
    }
}
