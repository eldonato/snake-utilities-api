using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SnakeUtilities.Modelos.EtiquetaCorreios;
using SnakeUtilities.Utils.Pdf.Componentes;

namespace SnakeUtilities.Utils.Pdf;

public class GeradorEtiquetaCorreiosPdf(Etiqueta etiqueta) : IDocument
{
    private const string Remetente = "Remetente";
    private const string Destinatario = "Destinatario";

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Header().Component(new ComponentePdfCabecalho());

            page.Content().PaddingTop(20).Column(column =>
            {
                column.Spacing(20);
                foreach (var destinatario in etiqueta.Destinatarios)
                {
                    if (etiqueta.Remetente != null)
                        column.Item().Component(new ComponenteEndereco(Remetente, etiqueta.Remetente));

                    column.Item().Component(new ComponenteEndereco(Destinatario, destinatario));
                }
            });
        });
    }
}