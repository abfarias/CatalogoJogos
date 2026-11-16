using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Admin
{
    public class AdicionarJogoModel : PageModel
    {
        // Enquanto não há login (MC03), os jogos são atribuídos ao administrador padrão criado na carga inicial.
        private const int IdAdminPadrao = 1;

        private readonly AdminCatalogoService _adminCatalogoService;

        [BindProperty]
        public Jogo Jogo { get; set; } = new();

        public AdicionarJogoModel(AdminCatalogoService adminCatalogoService)
        {
            _adminCatalogoService = adminCatalogoService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado = await _adminCatalogoService.CadastrarAsync(Jogo, IdAdminPadrao);
            if (!resultado.Sucesso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensagem);
                return Page();
            }

            TempData["Mensagem"] = resultado.Mensagem;
            return RedirectToPage("/Admin/Jogos");
        }
    }
}
