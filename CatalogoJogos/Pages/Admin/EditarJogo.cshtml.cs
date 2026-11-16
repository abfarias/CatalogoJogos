using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Admin
{
    public class EditarJogoModel : PageModel
    {
        private readonly AdminCatalogoService _adminCatalogoService;

        [BindProperty]
        public Jogo Jogo { get; set; } = new();

        public EditarJogoModel(AdminCatalogoService adminCatalogoService)
        {
            _adminCatalogoService = adminCatalogoService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var jogo = await _adminCatalogoService.ObterPorIdAsync(id);
            if (jogo is null)
            {
                TempData["Erro"] = "Jogo não encontrado.";
                return RedirectToPage("/Admin/Jogos");
            }

            Jogo = jogo;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado = await _adminCatalogoService.EditarAsync(id, Jogo);
            TempData[resultado.Sucesso ? "Mensagem" : "Erro"] = resultado.Mensagem;
            return RedirectToPage("/Admin/Jogos");
        }
    }
}
