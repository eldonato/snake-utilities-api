using System.Net;
using System.Net.Http.Headers;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/buscar-cep/{cep}", async (string cep, IServicoCorreios servico) => await servico.ObterEndereco(cep));

app.MapPost("/etiqueta-correios", (Etiqueta etiqueta, IServicoCorreios servico) =>
{
    var bytes = servico.GerarPdf(etiqueta);
    var result = new HttpResponseMessage(HttpStatusCode.OK);
    
    result.Content = new ByteArrayContent(bytes);
    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
    
    return result;
});

app.Run();