using SnakeUtilities.Modelos.BuscaCep;
using SnakeUtilities.Modelos.EtiquetaCorreios;

namespace SnakeUtilities.Servicos.Interfaces;

public interface IServicoCorreios
{
    public Task<EnderecoViaCep?> ObterEndereco(string? cep);

    public byte[] GerarPdf(Etiqueta etiqueta);
}