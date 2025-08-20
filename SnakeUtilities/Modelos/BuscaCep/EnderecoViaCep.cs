namespace SnakeUtilities.Modelos.BuscaCep;

public record EnderecoViaCep(
    string? Cep,
    string? Logradouro,
    string? Bairro,
    string? Localidade,
    string? Uf)
{
}