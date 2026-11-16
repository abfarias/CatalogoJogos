using System.ComponentModel.DataAnnotations;

namespace CatalogoJogos.Models
{
    public class Jogo
    {
        public int IdJogo { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Informe o título.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo {1} caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Informe a descrição.")]
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "Informe a categoria.")]
        [StringLength(50, ErrorMessage = "A categoria deve ter no máximo {1} caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        [Display(Name = "Preço (R$)")]
        [Required(ErrorMessage = "Informe o preço.")]
        [Range(typeof(decimal), "0", "9999.99", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "O preço deve estar entre R$ 0,00 e R$ 9.999,99.")]
        public decimal Preco { get; set; }

        [Display(Name = "Caminho / URL da capa")]
        [StringLength(500, ErrorMessage = "O caminho da capa deve ter no máximo {1} caracteres.")]
        public string? CaminhoFotoCapa { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; } = true;

        // Chave estrangeira da associação "Administrador gerencia Jogo".
        public int IdAdminCadastro { get; set; }
        public Administrador? AdminCadastro { get; set; }

        public List<Compra> Compras { get; set; } = new();
        public List<Avaliacao> Avaliacoes { get; set; } = new();

        public void AtualizarDados(string titulo, string descricao, string categoria, decimal preco, string? capa)
        {
            Titulo = titulo.Trim();
            Descricao = descricao.Trim();
            Categoria = categoria.Trim();
            Preco = preco;
            CaminhoFotoCapa = string.IsNullOrWhiteSpace(capa) ? null : capa.Trim();
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void Reativar()
        {
            Ativo = true;
        }
    }
}
