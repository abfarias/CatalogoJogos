using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CatalogoJogos.Services
{
    public class AutenticacaoService
    {
        private const string MensagemCredenciaisInvalidas = "E-mail ou senha inválidos.";

        private readonly IJogadorRepository _jogadorRepository;
        private readonly IAdministradorRepository _administradorRepository;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        private readonly IPasswordHasher<Jogador> _hasherJogador;
        private readonly IPasswordHasher<Administrador> _hasherAdministrador;

        public AutenticacaoService(
            IJogadorRepository jogadorRepository,
            IAdministradorRepository administradorRepository,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IPasswordHasher<Jogador> hasherJogador,
            IPasswordHasher<Administrador> hasherAdministrador)
        {
            _jogadorRepository = jogadorRepository;
            _administradorRepository = administradorRepository;
            _unidadeDeTrabalho = unidadeDeTrabalho;
            _hasherJogador = hasherJogador;
            _hasherAdministrador = hasherAdministrador;
        }

        public async Task<ResultadoOperacao<ClaimsPrincipal>> RegistrarJogadorAsync(string apelido, string email, string senha)
        {
            apelido = apelido.Trim();
            email = NormalizarEmail(email);

            // O login procura o e-mail nas duas tabelas, então ele não pode existir em nenhuma delas.
            if (await _jogadorRepository.ExisteEmailAsync(email)
                || await _administradorRepository.ObterPorEmailAsync(email) is not null)
                return ResultadoOperacao<ClaimsPrincipal>.Falha("E-mail já cadastrado.");

            if (await _jogadorRepository.ExisteApelidoAsync(apelido))
                return ResultadoOperacao<ClaimsPrincipal>.Falha("Apelido já está em uso.");

            var jogador = new Jogador
            {
                Apelido = apelido,
                Email = email,
                SaldoCarteira = 0m,
                DataCadastro = DateTime.Now
            };
            jogador.SenhaHash = _hasherJogador.HashPassword(jogador, senha);

            await _jogadorRepository.AdicionarAsync(jogador);
            await _unidadeDeTrabalho.SalvarAlteracoesAsync();

            return ResultadoOperacao<ClaimsPrincipal>.Ok(
                CriarPrincipal(jogador.IdJogador, jogador.Apelido, Perfis.Jogador),
                "Conta criada com sucesso.");
        }

        public async Task<ResultadoOperacao<ClaimsPrincipal>> AutenticarAsync(string email, string senha)
        {
            email = NormalizarEmail(email);

            var jogador = await _jogadorRepository.ObterPorEmailAsync(email);
            if (jogador is not null)
            {
                var resultado = _hasherJogador.VerifyHashedPassword(jogador, jogador.SenhaHash, senha);
                if (resultado == PasswordVerificationResult.Failed)
                    return ResultadoOperacao<ClaimsPrincipal>.Falha(MensagemCredenciaisInvalidas);

                if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    jogador.SenhaHash = _hasherJogador.HashPassword(jogador, senha);
                    await _unidadeDeTrabalho.SalvarAlteracoesAsync();
                }

                return ResultadoOperacao<ClaimsPrincipal>.Ok(
                    CriarPrincipal(jogador.IdJogador, jogador.Apelido, Perfis.Jogador));
            }

            var admin = await _administradorRepository.ObterPorEmailAsync(email);
            if (admin is not null)
            {
                var resultado = _hasherAdministrador.VerifyHashedPassword(admin, admin.SenhaHash, senha);
                if (resultado == PasswordVerificationResult.Failed)
                    return ResultadoOperacao<ClaimsPrincipal>.Falha(MensagemCredenciaisInvalidas);

                if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    admin.SenhaHash = _hasherAdministrador.HashPassword(admin, senha);
                    await _unidadeDeTrabalho.SalvarAlteracoesAsync();
                }

                return ResultadoOperacao<ClaimsPrincipal>.Ok(
                    CriarPrincipal(admin.IdAdmin, admin.Nome, Perfis.Administrador));
            }

            // Mesma mensagem para e-mail inexistente e senha errada, para não revelar quais e-mails existem.
            return ResultadoOperacao<ClaimsPrincipal>.Falha(MensagemCredenciaisInvalidas);
        }

        public static string NormalizarEmail(string email) => email.Trim().ToLowerInvariant();

        private static ClaimsPrincipal CriarPrincipal(int id, string nome, string perfil)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, id.ToString()),
                new(ClaimTypes.Name, nome),
                new(ClaimTypes.Role, perfil)
            };

            var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identidade);
        }
    }
}
