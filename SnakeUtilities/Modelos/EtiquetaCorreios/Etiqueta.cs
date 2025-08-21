namespace SnakeUtilities.Modelos.EtiquetaCorreios;

public class Etiqueta
{
    public DadosDeEnvio? Remetente { get; set; }
    public List<DadosDeEnvio> Destinatarios { get; set; } = [];
}