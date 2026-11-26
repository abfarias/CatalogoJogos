using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CatalogoJogos.Pages.Jogador
{
    public class CarteiraModel : PageModel
    {
        private readonly CarteiraService _carteiraService;

        public decimal Saldo { get; set; }

        [BindProperty]
        [Display(Name = "Valor da recarga (R$)")]
        [Required(ErrorMessage = "Informe o valor.")]
        [Range(typeof(decimal), "1", "1000", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "A recarga deve ser de R$ 1,00 a R$ 1.000,00.")]
        public decimal? Valor { get; set; }

        public CarteiraModel(CarteiraService carteiraService)
        {
            _carteiraService = carteiraService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var idJogador = User.ObterIdUsuario();
            if (idJogador is null)
                return Challenge();

            Saldo = await _carteiraService.ObterSaldoAsync(idJogador.Value);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idJogador = User.ObterIdUsuario();
            if (idJogador is null)
                return Challenge();

            if (!ModelState.IsValid)
            {
                Saldo = await _carteiraService.ObterSaldoAsync(idJogador.Value);
                return Page();
            }

            var resultado = await _carteiraService.AdicionarSaldoAsync(idJogador.Value, Valor!.Value);
            TempData[resultado.Sucesso ? "Mensagem" : "Erro"] = resultado.Mensagem;
            return RedirectToPage();
        }
    }
}
