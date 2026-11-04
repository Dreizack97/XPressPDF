namespace Core.Catalogs;

using System.Collections.Frozen;

/// <summary>
/// Catálogos oficiales del SAT para el complemento de Nómina 1.2 respaldados en FrozenDictionary.
/// </summary>
public static class NominaCatalogs
{
    public static readonly FrozenDictionary<string, string> TipoNomina = new Dictionary<string, string>
    {
        ["O"] = "Nómina ordinaria",
        ["E"] = "Nómina extraordinaria"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<string, string> TipoContrato = new Dictionary<string, string>
    {
        ["01"] = "Contrato de trabajo por tiempo indeterminado",
        ["02"] = "Contrato de trabajo para obra determinada",
        ["03"] = "Contrato de trabajo por tiempo determinado",
        ["04"] = "Contrato de trabajo por temporada",
        ["05"] = "Contrato de trabajo sujeto a prueba",
        ["06"] = "Contrato de trabajo con capacitación inicial",
        ["07"] = "Modalidad de contratación por pago de hora laborada",
        ["08"] = "Modalidad de trabajo por comisión laboral",
        ["09"] = "Modalidades de contratación donde no existe relación de trabajo",
        ["10"] = "Jubilación, pensión, retiro",
        ["99"] = "Otro contrato"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<string, string> TipoJornada = new Dictionary<string, string>
    {
        ["01"] = "Diurna",
        ["02"] = "Nocturna",
        ["03"] = "Mixta",
        ["04"] = "Por hora",
        ["05"] = "Reducida",
        ["06"] = "Continuada",
        ["07"] = "Partida",
        ["08"] = "Por turnos",
        ["99"] = "Otra Jornada"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<string, string> TipoRegimen = new Dictionary<string, string>
    {
        ["02"] = "Sueldos (Incluye ingresos señalados en la fracción I del artículo 94 de LISR)",
        ["03"] = "Jubilados",
        ["04"] = "Pensionados",
        ["05"] = "Asimilados Miembros Sociedades Cooperativas Producción",
        ["06"] = "Asimilados Integrantes Sociedades Asociaciones Civiles",
        ["07"] = "Asimilados Miembros consejos directivos, vigilancia, consultivos, honorarios administradores",
        ["08"] = "Asimilados comisionistas",
        ["09"] = "Asimilados Honorarios",
        ["10"] = "Asimilados acciones",
        ["11"] = "Asimilados otros",
        ["12"] = "Jubilados o Pensionados",
        ["13"] = "Indemnización o Separación",
        ["99"] = "Otro Régimen"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<string, string> PeriodicidadPago = new Dictionary<string, string>
    {
        ["01"] = "Diario",
        ["02"] = "Semanal",
        ["03"] = "Catorcenal",
        ["04"] = "Quincenal",
        ["05"] = "Mensual",
        ["06"] = "Bimestral",
        ["07"] = "Unidad de obra",
        ["08"] = "Comisión",
        ["09"] = "Precio alzado",
        ["10"] = "Decenal",
        ["99"] = "Otra Periodicidad"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<string, string> RiesgoPuesto = new Dictionary<string, string>
    {
        ["1"] = "Clase I",
        ["2"] = "Clase II",
        ["3"] = "Clase III",
        ["4"] = "Clase IV",
        ["5"] = "Clase V",
        ["99"] = "No aplica"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<string, string> Banco = new Dictionary<string, string>
    {
        ["002"] = "BANAMEX",
        ["012"] = "BBVA BANCOMER",
        ["014"] = "SANTANDER",
        ["021"] = "HSBC",
        ["030"] = "BAJIO",
        ["036"] = "INBURSA",
        ["044"] = "SCOTIABANK",
        ["058"] = "BANREGIO",
        ["072"] = "BANORTE",
        ["127"] = "AZTECA",
        ["137"] = "BANCOPPEL"
    }.ToFrozenDictionary();
}
