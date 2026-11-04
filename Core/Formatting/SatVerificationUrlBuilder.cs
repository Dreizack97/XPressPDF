namespace Core.Formatting;

using Core.Models.Base;
using System.Globalization;

/// <summary>
/// Generador de la URL de verificación del SAT para el código QR conforme al Anexo 20 (Rubro I.D).
/// </summary>
public static class SatVerificationUrlBuilder
{
    private const string SatVerificationBaseUrl = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";

    public static string Build(Comprobante comprobante)
    {
        string uuid = comprobante.TimbreFiscalDigital?.UUID ?? string.Empty;
        string re = comprobante.Emisor?.Rfc ?? string.Empty;
        string rr = comprobante.Receptor?.Rfc ?? string.Empty;

        // El formato de total para la URL del SAT requiere punto decimal y sin separadores de miles
        string tt = comprobante.Total.ToString("0.000000", CultureInfo.InvariantCulture).TrimEnd('0');
        if (tt.EndsWith('.'))
            tt += "00";
        else if (tt.IndexOf('.') == tt.Length - 2)
            tt += "0";

        // El parámetro 'fe' corresponde a los últimos 8 caracteres del sello del comprobante
        string? sello = !string.IsNullOrWhiteSpace(comprobante.TimbreFiscalDigital?.SelloCFD)
            ? comprobante.TimbreFiscalDigital.SelloCFD
            : comprobante.Sello;

        string fe = sello?.Length >= 8 ? sello[^8..] : sello ?? string.Empty;

        return $"{SatVerificationBaseUrl}?id={uuid}&re={Uri.EscapeDataString(re)}&rr={Uri.EscapeDataString(rr)}&tt={tt}&fe={Uri.EscapeDataString(fe)}";
    }
}
