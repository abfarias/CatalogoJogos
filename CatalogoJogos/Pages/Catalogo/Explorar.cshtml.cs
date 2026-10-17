using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace CatalogoJogos.Pages.Catalogo
{
    public class ExplorarModel : PageModel
    {
        private readonly IRepository<Jogo> _jogoRepository;
        [BindProperty]
        public List<Jogo> JogosDisponiveis { get; set; } = new();

        public ExplorarModel(IRepository<Jogo> jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public void OnGet()
        {
            JogosDisponiveis = _jogoRepository.ListarTodos();
        }
    }
}
