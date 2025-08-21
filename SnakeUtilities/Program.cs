using QuestPDF.Infrastructure;
using SnakeUtilities.Modelos.EtiquetaCorreios;
using SnakeUtilities.Servicos;
using SnakeUtilities.Servicos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServicoCorreios, ServicoCorreios>();

builder.Services.AddHttpClient();

QuestPDF.Settings.License = LicenseType.Community;

var corsPadrao = "CorsPadrao";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPadrao,
        policy =>
        {
            // Para desenvolvimento, você pode ser mais permissivo
            // Lembre-se de ser mais restritivo em produção!
            policy.WithOrigins("http://localhost:3000") // A URL do seu frontend Vue
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors(corsPadrao);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/correios/buscar-cep/{cep}", async (string cep, IServicoCorreios servico) => await servico.ObterEndereco(cep));

app.MapPost("/correios/etiqueta-pdf", (Etiqueta etiqueta, IServicoCorreios servico) =>
{
    var bytes = servico.GerarPdf(etiqueta);
    
    var nomeArquivo = $"etiqueta_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
    
    return Results.File(bytes, "application/pdf", nomeArquivo);
});

app.Run();