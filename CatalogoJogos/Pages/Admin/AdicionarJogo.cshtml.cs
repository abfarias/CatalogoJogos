using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoJogos.Pages.Admin
{
    public class AdicionarJogoModel : PageModel
    {
        private readonly IRepository<Jogo> _jogoRepository;
        [BindProperty]
        public Jogo NovoJogo { get; set; } = new();

        public AdicionarJogoModel(IRepository<Jogo> jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _jogoRepository.Adicionar(NovoJogo);
            return RedirectToPage("/Catalogo/Explorar");
        }
    }
}
