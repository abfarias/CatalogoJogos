namespace CatalogoJogos.Models
{
    public class Jogo
    {
        public int IdJogo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string CaminhoFotoCapa { get; set; } = string.Empty;
    }
}
