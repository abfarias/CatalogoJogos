using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Admin
{
    public class AdicionarJogoModel : PageModel
    {
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

            // A pasta /Admin exige o perfil Administrador, então o Id vem do cookie.
            var idAdmin = User.ObterIdUsuario();
            if (idAdmin is null)
                return Challenge();

            var resultado = await _adminCatalogoService.CadastrarAsync(Jogo, idAdmin.Value);
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
