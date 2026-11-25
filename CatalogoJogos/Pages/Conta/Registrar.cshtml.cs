using CatalogoJogos.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CatalogoJogos.Pages.Conta
{
    public class RegistrarModel : PageModel
    {
        private readonly AutenticacaoService _autenticacaoService;

        [BindProperty]
        public DadosCadastro Dados { get; set; } = new();

        public RegistrarModel(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var resultado = await _autenticacaoService.RegistrarJogadorAsync(Dados.Apelido, Dados.Email, Dados.Senha);
            if (!resultado.Sucesso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensagem);
                return Page();
            }

            await HttpContext.SignInAsync(resultado.Valor!);
            return RedirectToPage("/Catalogo/Explorar");
        }

        public class DadosCadastro
        {
            [Display(Name = "Apelido")]
            [Required(ErrorMessage = "Informe o apelido.")]
            [StringLength(50, ErrorMessage = "O apelido deve ter no máximo {1} caracteres.")]
            public string Apelido { get; set; } = string.Empty;

            [Display(Name = "E-mail")]
            [Required(ErrorMessage = "Informe o e-mail.")]
            [EmailAddress(ErrorMessage = "E-mail inválido.")]
            [StringLength(256, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres.")]
            public string Email { get; set; } = string.Empty;

            [Display(Name = "Senha")]
            [Required(ErrorMessage = "Informe a senha.")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre {2} e {1} caracteres.")]
            [DataType(DataType.Password)]
            public string Senha { get; set; } = string.Empty;

            [Display(Name = "Confirmar senha")]
            [Required(ErrorMessage = "Confirme a senha.")]
            [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem.")]
            [DataType(DataType.Password)]
            public string ConfirmarSenha { get; set; } = string.Empty;
        }
    }
}
