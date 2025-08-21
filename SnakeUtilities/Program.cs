using QuestPDF;
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

Settings.License = LicenseType.Community;

var corsPadrao = "CorsPadrao";
var origensPermitidas = builder.Configuration.GetSection("OriginsPermitidas").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPadrao,
        policy =>
        {
            if (origensPermitidas?.Length > 0)
                policy.WithOrigins(origensPermitidas)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors(corsPadrao);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.MapGet("/correios/buscar-cep/{cep}",
    async (string cep, IServicoCorreios servico) =>
    {
        try
        {
            return Results.Ok(await servico.ObterEndereco(cep));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return Results.Problem("Ocorreu um erro ao buscar o CEP.", statusCode: 500);
        }
    });

app.MapPost("/correios/etiqueta-pdf",
    (Etiqueta etiqueta, IServicoCorreios servico) =>
    {
        try
        {
            var bytes = servico.GerarPdf(etiqueta);

            var nomeArquivo = $"etiqueta_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";

            return Results.File(bytes, "application/pdf", nomeArquivo);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return Results.Problem("Ocorreu um erro ao gerar a etiqueta.", statusCode: 500);
        }
    });

app.Run();