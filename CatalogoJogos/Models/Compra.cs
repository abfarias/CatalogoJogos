namespace CatalogoJogos.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal ValorDebitado { get; set; }
        public int IdJogador { get; set; }
        public int IdJogo { get; set; }
    }
}
