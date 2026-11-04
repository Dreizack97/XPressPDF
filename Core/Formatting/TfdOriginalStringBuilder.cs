namespace Core.Formatting;

using Core.Models.Complements.TimbreFiscalDigital;
using System.Text;

/// <summary>
/// Generador de la Cadena Original del Timbre Fiscal Digital versión 1.1 según la especificación técnica oficial del SAT (cadenaoriginal_TFD_1_1.xslt).
/// </summary>
public static class TfdOriginalStringBuilder
{
    public static string Build(TimbreFiscalDigital? tfd)
    {
        if (tfd == null)
            return string.Empty;

        StringBuilder sb = new();
        sb.Append("||");
        sb.Append(tfd.Version);
        sb.Append('|');
        sb.Append(tfd.UUID);
        sb.Append('|');
        sb.Append(tfd.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss"));
        sb.Append('|');
        sb.Append(tfd.RfcProvCertif);
        sb.Append('|');

        if (!string.IsNullOrWhiteSpace(tfd.Leyenda))
        {
            sb.Append(tfd.Leyenda);
            sb.Append('|');
        }

        sb.Append(tfd.SelloCFD);
        sb.Append('|');
        sb.Append(tfd.NoCertificadoSAT);
        sb.Append("||");

        return sb.ToString();
    }
}
