using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Admin
{
    public class JogosModel : PageModel
    {
        private readonly AdminCatalogoService _adminCatalogoService;

        public List<Jogo> Jogos { get; set; } = new();

        public JogosModel(AdminCatalogoService adminCatalogoService)
        {
            _adminCatalogoService = adminCatalogoService;
        }

        public async Task OnGetAsync()
        {
            Jogos = await _adminCatalogoService.ListarTodosAsync();
        }

        public async Task<IActionResult> OnPostAlterarSituacaoAsync(int id)
        {
            var resultado = await _adminCatalogoService.AlterarSituacaoAsync(id);
            TempData[resultado.Sucesso ? "Mensagem" : "Erro"] = resultado.Mensagem;
            return RedirectToPage();
        }
    }
}
