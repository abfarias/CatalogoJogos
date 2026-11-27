using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Catalogo
{
    public class ExplorarModel : PageModel
    {
        private readonly CatalogoService _catalogoService;
        private readonly CompraService _compraService;

        public List<Jogo> JogosDisponiveis { get; set; } = new();
        public List<string> Categorias { get; set; } = new();

        // Filtros vêm da query string (?Termo=...&Categoria=...) para continuarem preenchidos.
        [BindProperty(SupportsGet = true)]
        public string? Termo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Categoria { get; set; }

        public bool FiltroAplicado => !string.IsNullOrWhiteSpace(Termo) || !string.IsNullOrWhiteSpace(Categoria);

        // Jogos que o jogador logado já possui (vazio para visitantes e administradores).
        public HashSet<int> IdsNaBiblioteca { get; set; } = new();

        public ExplorarModel(CatalogoService catalogoService, CompraService compraService)
        {
            _catalogoService = catalogoService;
            _compraService = compraService;
        }

        public async Task OnGetAsync()
        {
            Categorias = await _catalogoService.ListarCategoriasAsync();
            JogosDisponiveis = await _catalogoService.BuscarAsync(Termo, Categoria);

            var idJogador = User.EhJogador() ? User.ObterIdUsuario() : null;
            if (idJogador is not null)
            {
                var compras = await _compraService.ListarBibliotecaAsync(idJogador.Value);
                IdsNaBiblioteca = compras.Select(c => c.IdJogo).ToHashSet();
            }
        }

        public async Task<IActionResult> OnPostComprarAsync(int idJogo)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Conta/Login", new { ReturnUrl = Url.Page("/Catalogo/Explorar") });

            var idJogador = User.ObterIdUsuario();
            if (!User.EhJogador() || idJogador is null)
                return Forbid();

            var resultado = await _compraService.ComprarAsync(idJogador.Value, idJogo);
            if (!resultado.Sucesso)
            {
                TempData["Erro"] = resultado.Mensagem;
                return RedirectToPage();
            }

            TempData["Mensagem"] = resultado.Mensagem;
            return RedirectToPage("/Jogador/Biblioteca");
        }
    }
}
