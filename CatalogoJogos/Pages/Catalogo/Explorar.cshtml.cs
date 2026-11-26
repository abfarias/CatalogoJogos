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

        // Jogos que o jogador logado já possui (vazio para visitantes e administradores).
        public HashSet<int> IdsNaBiblioteca { get; set; } = new();

        public ExplorarModel(CatalogoService catalogoService, CompraService compraService)
        {
            _catalogoService = catalogoService;
            _compraService = compraService;
        }

        public async Task OnGetAsync()
        {
            JogosDisponiveis = await _catalogoService.BuscarAsync(termo: null, categoria: null);

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
