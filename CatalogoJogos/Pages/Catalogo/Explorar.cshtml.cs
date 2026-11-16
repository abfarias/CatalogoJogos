using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Catalogo
{
    public class ExplorarModel : PageModel
    {
        private readonly CatalogoService _catalogoService;
        public List<Jogo> JogosDisponiveis { get; set; } = new();

        public ExplorarModel(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public async Task OnGetAsync()
        {
            JogosDisponiveis = await _catalogoService.BuscarAsync(termo: null, categoria: null);
        }
    }
}
