using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Models;
using CatalogoJogos.Repositories;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    // Páginas restritas por perfil (claim Role do cookie).
    options.Conventions.AuthorizeFolder("/Admin", Perfis.Administrador);
    options.Conventions.AuthorizeFolder("/Jogador", Perfis.Jogador);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Conta/Login";
        options.AccessDeniedPath = "/Conta/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Perfis.Administrador, policy => policy.RequireRole(Perfis.Administrador))
    .AddPolicy(Perfis.Jogador, policy => policy.RequireRole(Perfis.Jogador));

builder.Services.AddSingleton<IPasswordHasher<Jogador>, PasswordHasher<Jogador>>();
builder.Services.AddSingleton<IPasswordHasher<Administrador>, PasswordHasher<Administrador>>();

// Em desenvolvimento, a connection string vem do user-secrets.
var connectionString = builder.Configuration.GetConnectionString("CatalogoJogos")
    ?? throw new InvalidOperationException(
        "Connection string 'CatalogoJogos' não configurada. Use: dotnet user-secrets set \"ConnectionStrings:CatalogoJogos\" \"<connection string>\"");

builder.Services.AddDbContext<CatalogoJogosDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnidadeDeTrabalho>(sp => sp.GetRequiredService<CatalogoJogosDbContext>());

builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IJogadorRepository, JogadorRepository>();
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();

builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<CatalogoService>();
builder.Services.AddScoped<AdminCatalogoService>();
builder.Services.AddScoped<CarteiraService>();
builder.Services.AddScoped<CompraService>();

var app = builder.Build();

await DbInicializador.InicializarAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Os campos numéricos do HTML (type="number") enviam decimais com ponto (ex.: 12.50).
// Com a cultura invariável, o model binding lê o valor corretamente; a exibição em R$
// é feita explicitamente com a cultura pt-BR nas páginas.
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture),
    SupportedCultures = [CultureInfo.InvariantCulture],
    SupportedUICultures = [CultureInfo.InvariantCulture]
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
