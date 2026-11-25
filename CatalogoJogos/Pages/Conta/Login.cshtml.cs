using CatalogoJogos.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CatalogoJogos.Pages.Conta
{
    public class LoginModel : PageModel
    {
        private readonly AutenticacaoService _autenticacaoService;

        [BindProperty]
        public DadosLogin Dados { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public LoginModel(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado = await _autenticacaoService.AutenticarAsync(Dados.Email, Dados.Senha);
            if (!resultado.Sucesso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensagem);
                return Page();
            }

            await HttpContext.SignInAsync(resultado.Valor!);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return LocalRedirect(ReturnUrl);

            return resultado.Valor!.EhAdministrador()
                ? RedirectToPage("/Admin/Jogos")
                : RedirectToPage("/Catalogo/Explorar");
        }

        public class DadosLogin
        {
            [Display(Name = "E-mail")]
            [Required(ErrorMessage = "Informe o e-mail.")]
            [EmailAddress(ErrorMessage = "E-mail inválido.")]
            public string Email { get; set; } = string.Empty;

            [Display(Name = "Senha")]
            [Required(ErrorMessage = "Informe a senha.")]
            [DataType(DataType.Password)]
            public string Senha { get; set; } = string.Empty;
        }
    }
}
