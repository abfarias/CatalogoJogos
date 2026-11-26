namespace CatalogoJogos.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal ValorDebitado { get; set; }
        public int IdJogador { get; set; }
        public int IdJogo { get; set; }

        public Jogador? Jogador { get; set; }
        public Jogo? Jogo { get; set; }

        // Registra o preço do momento da compra; mudanças futuras no preço não alteram a compra.
        public static Compra Criar(Jogador jogador, Jogo jogo)
        {
            return new Compra
            {
                IdJogador = jogador.IdJogador,
                IdJogo = jogo.IdJogo,
                Jogador = jogador,
                Jogo = jogo,
                ValorDebitado = jogo.Preco,
                DataAquisicao = DateTime.Now
            };
        }
    }
}
