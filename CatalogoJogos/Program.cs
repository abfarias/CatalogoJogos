using CatalogoJogos.Data;
using CatalogoJogos.Interfaces;
using CatalogoJogos.Repositories;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

builder.Services.AddScoped<CatalogoService>();
builder.Services.AddScoped<AdminCatalogoService>();

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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
