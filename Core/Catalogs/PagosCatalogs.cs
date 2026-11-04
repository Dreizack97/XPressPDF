namespace Core.Catalogs;

using System.Collections.Frozen;

/// <summary>
/// Catálogos del SAT para el complemento de Recepción de Pagos 2.0.
/// </summary>
public static class PagosCatalogs
{
    public static readonly FrozenDictionary<string, string> TipoCadenaPago = new Dictionary<string, string>
    {
        ["01"] = "SPEI"
    }.ToFrozenDictionary();
}
