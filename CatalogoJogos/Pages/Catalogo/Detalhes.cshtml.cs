using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CatalogoJogos.Pages.Catalogo
{
    public class DetalhesModel : PageModel
    {
        private readonly CatalogoService _catalogoService;
        private readonly CompraService _compraService;
        private readonly AvaliacaoService _avaliacaoService;

        public DetalhesJogo Detalhes { get; set; } = null!;

        // Avaliação do jogador logado, se já existir (usada para preencher o formulário).
        public Avaliacao? MinhaAvaliacao { get; set; }

        [BindProperty]
        public DadosAvaliacao NovaAvaliacao { get; set; } = new();

        public DetalhesModel(CatalogoService catalogoService, CompraService compraService, AvaliacaoService avaliacaoService)
        {
            _catalogoService = catalogoService;
            _compraService = compraService;
            _avaliacaoService = avaliacaoService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!await CarregarAsync(id))
                return JogoNaoEncontrado();

            if (MinhaAvaliacao is not null)
            {
                NovaAvaliacao.Nota = MinhaAvaliacao.Nota;
                NovaAvaliacao.Comentario = MinhaAvaliacao.Comentario;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostComprarAsync(int id)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Conta/Login", new { ReturnUrl = Url.Page("/Catalogo/Detalhes", new { id }) });

            var idJogador = User.ObterIdUsuario();
            if (!User.EhJogador() || idJogador is null)
                return Forbid();

            var resultado = await _compraService.ComprarAsync(idJogador.Value, id);
            if (!resultado.Sucesso)
            {
                TempData["Erro"] = resultado.Mensagem;
                return RedirectToPage(new { id });
            }

            TempData["Mensagem"] = resultado.Mensagem;
            return RedirectToPage("/Jogador/Biblioteca");
        }

        public async Task<IActionResult> OnPostAvaliarAsync(int id)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Conta/Login", new { ReturnUrl = Url.Page("/Catalogo/Detalhes", new { id }) });

            var idJogador = User.ObterIdUsuario();
            if (!User.EhJogador() || idJogador is null)
                return Forbid();

            if (!ModelState.IsValid)
            {
                if (!await CarregarAsync(id))
                    return JogoNaoEncontrado();
                return Page();
            }

            var resultado = await _avaliacaoService.AvaliarAsync(
                idJogador.Value, id, NovaAvaliacao.Nota!.Value, NovaAvaliacao.Comentario);
            TempData[resultado.Sucesso ? "Mensagem" : "Erro"] = resultado.Mensagem;
            return RedirectToPage(new { id });
        }

        private async Task<bool> CarregarAsync(int id)
        {
            var idJogador = User.EhJogador() ? User.ObterIdUsuario() : null;
            var detalhes = await _catalogoService.ObterDetalhesAsync(id, idJogador);
            if (detalhes is null)
                return false;

            Detalhes = detalhes;
            MinhaAvaliacao = idJogador is null ? null : detalhes.Avaliacoes.FirstOrDefault(a => a.IdJogador == idJogador);
            return true;
        }

        private IActionResult JogoNaoEncontrado()
        {
            TempData["Erro"] = "Jogo não encontrado.";
            return RedirectToPage("/Catalogo/Explorar");
        }

        public class DadosAvaliacao
        {
            [Display(Name = "Nota")]
            [Required(ErrorMessage = "Escolha uma nota.")]
            [Range(1, 5, ErrorMessage = "A nota deve ser de 1 a 5.")]
            public int? Nota { get; set; }

            [Display(Name = "Comentário (opcional)")]
            [StringLength(500, ErrorMessage = "O comentário deve ter no máximo {1} caracteres.")]
            public string? Comentario { get; set; }
        }
    }
}
