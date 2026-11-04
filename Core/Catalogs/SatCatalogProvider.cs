namespace Core.Catalogs;

using System.Collections.Frozen;

/// <summary>
/// Fachada unificada y de alto rendimiento para la consulta y descripción de catálogos del SAT.
/// </summary>
public static class SatCatalogProvider
{
    public static string GetFormaPago(string? code) => Describe(CfdiBaseCatalogs.FormaPago, code);

    public static string GetMetodoPago(string? code) => Describe(CfdiBaseCatalogs.MetodoPago, code);

    public static string GetUsoCfdi(string? code) => Describe(CfdiBaseCatalogs.UsoCfdi, code);

    public static string GetRegimenFiscal(string? code) => Describe(CfdiBaseCatalogs.RegimenFiscal, code);

    public static string GetTipoComprobante(string? code) => Describe(CfdiBaseCatalogs.TipoComprobante, code);

    public static string GetTipoComprobanteName(string? code) =>
        !string.IsNullOrWhiteSpace(code) && CfdiBaseCatalogs.TipoComprobante.TryGetValue(code, out string? name)
            ? name
            : code ?? string.Empty;

    public static string GetImpuesto(string? code) => Describe(CfdiBaseCatalogs.Impuesto, code);

    public static string GetExportacion(string? code) => Describe(CfdiBaseCatalogs.Exportacion, code);

    public static string GetObjetoImp(string? code) => Describe(CfdiBaseCatalogs.ObjetoImp, code);

    public static string GetTipoRelacion(string? code) => Describe(CfdiBaseCatalogs.TipoRelacion, code);

    public static string GetPeriodicidad(string? code) => Describe(CfdiBaseCatalogs.Periodicidad, code);

    public static string GetMeses(string? code) => Describe(CfdiBaseCatalogs.Meses, code);

    // --- Nómina ---
    public static string GetTipoNomina(string? code) => Describe(NominaCatalogs.TipoNomina, code);

    public static string GetTipoContrato(string? code) => Describe(NominaCatalogs.TipoContrato, code);

    public static string GetTipoJornada(string? code) => Describe(NominaCatalogs.TipoJornada, code);

    public static string GetTipoRegimen(string? code) => Describe(NominaCatalogs.TipoRegimen, code);

    public static string GetPeriodicidadPago(string? code) => Describe(NominaCatalogs.PeriodicidadPago, code);

    public static string GetRiesgoPuesto(string? code) => Describe(NominaCatalogs.RiesgoPuesto, code);

    public static string GetBanco(string? code) => Describe(NominaCatalogs.Banco, code);

    private static string Describe(FrozenDictionary<string, string> catalog, string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return string.Empty;

        return catalog.TryGetValue(code, out string? description)
            ? $"{code} - {description}"
            : code;
    }
}
