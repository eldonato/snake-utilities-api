using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SnakeUtilities.Utils.Pdf.Componentes;

public class ComponentePdfCabecalho : IComponent
{
    public void Compose(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item()
                    .PaddingBottom(30)
                    .AlignCenter()
                    .Text("FORNECIMENTO SNAKE TEAM 🐍🐍🐍🐍🐍")
                    .FontSize(20)
                    .SemiBold()
                    .FontColor(Colors.Green.Darken3);
            });
        });
    }
}