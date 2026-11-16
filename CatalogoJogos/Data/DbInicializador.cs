using CatalogoJogos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CatalogoJogos.Data
{
    // Aplica as migrations e faz a carga inicial: administrador padrão + jogos de Data/jogos.csv.
    public static class DbInicializador
    {
        public static async Task InicializarAsync(IServiceProvider services)
        {
            using var escopo = services.CreateScope();
            var contexto = escopo.ServiceProvider.GetRequiredService<CatalogoJogosDbContext>();
            var configuracao = escopo.ServiceProvider.GetRequiredService<IConfiguration>();
            var ambiente = escopo.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            await contexto.Database.MigrateAsync();

            var admin = await contexto.Administradores.OrderBy(a => a.IdAdmin).FirstOrDefaultAsync();
            if (admin is null)
            {
                admin = CriarAdministradorPadrao(configuracao);
                contexto.Administradores.Add(admin);
            }

            if (!await contexto.Jogos.AnyAsync())
            {
                var caminhoCsv = Path.Combine(ambiente.ContentRootPath, "Data", "jogos.csv");
                contexto.Jogos.AddRange(LerJogosDoCsv(caminhoCsv, admin));
            }

            await contexto.SaveChangesAsync();
        }

        // As credenciais do administrador padrão vêm do user-secrets (seção AdminPadrao).
        private static Administrador CriarAdministradorPadrao(IConfiguration configuracao)
        {
            var secao = configuracao.GetSection("AdminPadrao");
            var admin = new Administrador
            {
                Nome = secao["Nome"] ?? "Administrador",
                Email = ObterObrigatorio(secao, "Email")
            };
            var senha = ObterObrigatorio(secao, "Senha");
            admin.SenhaHash = new PasswordHasher<Administrador>().HashPassword(admin, senha);
            return admin;
        }

        private static string ObterObrigatorio(IConfigurationSection secao, string chave)
        {
            var valor = secao[chave];
            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException(
                    $"'AdminPadrao:{chave}' não configurado. Use: dotnet user-secrets set \"AdminPadrao:{chave}\" \"<valor>\"");
            return valor;
        }

        // Formato: IdJogo;Titulo;Descricao;Categoria;Preco;CaminhoFotoCapa (o Id é gerado pelo banco).
        private static IEnumerable<Jogo> LerJogosDoCsv(string caminho, Administrador admin)
        {
            if (!File.Exists(caminho))
                yield break;

            var agora = DateTime.Now;
            foreach (var linha in File.ReadLines(caminho).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;

                var campos = linha.Split(';');
                yield return new Jogo
                {
                    Titulo = campos[1],
                    Descricao = campos[2],
                    Categoria = campos[3],
                    Preco = decimal.Parse(campos[4], CultureInfo.InvariantCulture),
                    CaminhoFotoCapa = string.IsNullOrWhiteSpace(campos[5]) ? null : campos[5],
                    DataCadastro = agora,
                    Ativo = true,
                    AdminCadastro = admin
                };
            }
        }
    }
}
