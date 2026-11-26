using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Jogador
{
    public class BibliotecaModel : PageModel
    {
        private readonly CompraService _compraService;

        public List<Compra> Compras { get; set; } = new();

        public BibliotecaModel(CompraService compraService)
        {
            _compraService = compraService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var idJogador = User.ObterIdUsuario();
            if (idJogador is null)
                return Challenge();

            Compras = await _compraService.ListarBibliotecaAsync(idJogador.Value);
            return Page();
        }
    }
}
