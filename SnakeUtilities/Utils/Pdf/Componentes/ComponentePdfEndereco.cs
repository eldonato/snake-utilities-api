using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SnakeUtilities.Modelos.EtiquetaCorreios;

namespace SnakeUtilities.Utils.Pdf.Componentes;

public class ComponenteEndereco(string titulo, Secao secao) : IComponent
{
    public void Compose(IContainer container)
    {
        container
            .AlignCenter()
            .Width(400)
            .Border(1)
            .PaddingHorizontal(5)
            .PaddingVertical(10)
            .DefaultTextStyle(text => text.FontSize(12).FontFamily("Arial Black"))
            .Column(col =>
            {
                col.Item().Text(titulo).Bold();
                
                col.Item().Row(row =>
                {
                    row.RelativeItem(0.7F).Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(secao.Pessoa?.Nome);
                    row.RelativeItem(0.3F).Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(FormatarCelular(secao.Pessoa?.Telefone));
                    row.Spacing(5);
                });
                
                col.Item().Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(FormatarLogradouro(secao.Endereco));
                
                col.Item().Row(row =>
                {
                    row.RelativeItem(0.2F).Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(FormatarCep(secao.Endereco?.Cep));
                    row.RelativeItem(0.3F).Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(secao.Endereco?.Cidade);
                    row.RelativeItem(0.25F).Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(FormatarCelular(secao.Endereco?.Uf));
                    row.RelativeItem(0.25F).Border(1).PaddingHorizontal(5).PaddingVertical(1).Text(FormatarCelular(secao.Endereco?.Pais));
                    row.Spacing(5);
                });
                
                col.Spacing(10);
            });
    }

    private static string FormatarLogradouro(Endereco? endereco)
    {
        if (endereco == null) return string.Empty;
        
        var rua =  endereco.Logradouro;
        var numero = string.IsNullOrEmpty(endereco.Numero) ? "S/N" : $"nº {endereco.Numero}";
        var bairro = endereco.Bairro;
        var complemento =  string.IsNullOrEmpty(endereco.Complemento) ? string.Empty : $", {endereco.Complemento}";

        return $"{rua}, {numero} - {bairro}{complemento}";
    }
    
    private static string FormatarCelular(string? numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return string.Empty;

        var apenasDigitos = numero.ApenasDigitos();

        if (apenasDigitos.Length != 11)
        {
            return numero; 
        }

        try
        {
            var numeroLong = Convert.ToInt64(apenasDigitos);
            return numeroLong.ToString("(##) # ####-####");
        }
        catch (FormatException)
        {
            return numero;
        }
    }

    private static string FormatarCep(string? cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return string.Empty;

        var apenasDigitos = cep.ApenasDigitos();

        if (apenasDigitos.Length != 8)
        {
            return cep; 
        }

        var parte1 = apenasDigitos.Substring(0, 2);
        var parte2 = apenasDigitos.Substring(2, 3);
        var parte3 = apenasDigitos.Substring(5, 3);
        
        return $"{parte1}.{parte2}-{parte3}";
    }
}