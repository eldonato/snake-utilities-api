namespace SnakeUtilities.Modelos.EtiquetaCorreios;

public class Etiqueta
{
    public Secao? Remetente { get; set; }
    public List<Secao> Destinatarios { get; set; } = [];
}