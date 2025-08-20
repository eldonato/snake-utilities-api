using QuestPDF.Fluent;
using SnakeUtilities.Modelos.BuscaCep;
using SnakeUtilities.Modelos.EtiquetaCorreios;
using SnakeUtilities.Servicos.Interfaces;
using SnakeUtilities.Utils;
using SnakeUtilities.Utils.Pdf;

namespace SnakeUtilities.Servicos;

public class ServicoCorreios(HttpClient httpClient) : IServicoCorreios
{
    public async Task<EnderecoViaCep?> ObterEndereco(string? cep)
    {
        cep = cep.ApenasDigitos();

        if (cep.Length != 8) return null;
        
        Console.WriteLine("Realizando busca para o cep: {0}", cep);

        try
        {
            var resultado = await httpClient.GetFromJsonAsync<EnderecoViaCep>($"https://viacep.com.br/ws/{cep}/json/");

            Console.WriteLine("Busca realizada com sucesso");
            return resultado;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null;
        }
    }

    public byte[] GerarPdf(Etiqueta etiqueta)
    {
        var documento = new GeradorEtiquetaCorreiosPdf(etiqueta);

        return documento.GeneratePdf();
    }
}