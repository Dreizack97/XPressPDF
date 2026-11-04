namespace Core.Formatting;

/// <summary>
/// Convierte importes numéricos a su representación en letra en idioma español,
/// conforme a los lineamientos fiscales para representaciones impresas de CFDI.
/// </summary>
public static class CurrencyToWordsConverter
{
    private static readonly string[] Units =
    [
        "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE", "DIEZ",
        "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISÉIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE",
        "VEINTE", "VEINTIÚN", "VEINTIDÓS", "VEINTITRÉS", "VEINTICUATRO", "VEINTICINCO",
        "VEINTISÉIS", "VEINTISIETE", "VEINTIOCHO", "VEINTINUEVE"
    ];

    private static readonly string[] Tens =
    [
        "", "", "", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA"
    ];

    private static readonly string[] Hundreds =
    [
        "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS",
        "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS"
    ];

    public static string ToText(decimal amount, string? currency = "MXN")
    {
        long integerPart = (long)Math.Truncate(amount);
        int cents = (int)Math.Round((amount - integerPart) * 100, MidpointRounding.AwayFromZero);

        if (cents == 100)
        {
            integerPart++;
            cents = 0;
        }

        string normalizedCurrency = string.IsNullOrWhiteSpace(currency) ? "MXN" : currency.Trim().ToUpperInvariant();

        // En español, si la cantidad termina en millones exactos sin miles ni unidades, se intercala "DE"
        bool requiresDe = integerPart >= 1_000_000 && (integerPart % 1_000_000 == 0);

        string currencyName = normalizedCurrency switch
        {
            "MXN" => integerPart == 1 ? "PESO" : (requiresDe ? "DE PESOS" : "PESOS"),
            "USD" => integerPart == 1 ? "DÓLAR" : (requiresDe ? "DE DÓLARES" : "DÓLARES"),
            "EUR" => integerPart == 1 ? "EURO" : (requiresDe ? "DE EUROS" : "EUROS"),
            "CAD" => integerPart == 1 ? "DÓLAR CANADIENSE" : (requiresDe ? "DE DÓLARES CANADIENSES" : "DÓLARES CANADIENSES"),
            "GBP" => integerPart == 1 ? "LIBRA ESTERLINA" : (requiresDe ? "DE LIBRAS ESTERLINAS" : "LIBRAS ESTERLINAS"),
            "XXX" => string.Empty,
            _ => requiresDe ? $"DE {normalizedCurrency}" : normalizedCurrency
        };

        string suffix = normalizedCurrency is "MXN" or "" ? "M.N." : (normalizedCurrency == "XXX" ? string.Empty : "M.E.");

        string words = ToWords(integerPart);
        string parts = string.Join(" ", new[] { words, currencyName, $"{cents:00}/100", suffix }.Where(s => !string.IsNullOrEmpty(s)));

        return $"({parts})";
    }

    private static string ToWords(long number)
    {
        if (number == 0)
            return "CERO";

        List<string> parts = [];

        long millions = number / 1_000_000;
        long remainder = number % 1_000_000;

        if (millions > 0)
            parts.Add(millions == 1 ? "UN MILLÓN" : $"{ToWords(millions)} MILLONES");

        long thousands = remainder / 1_000;
        long hundreds = remainder % 1_000;

        if (thousands > 0)
            parts.Add(thousands == 1 ? "MIL" : $"{GroupToWords(thousands)} MIL");

        if (hundreds > 0)
            parts.Add(GroupToWords(hundreds));

        return string.Join(" ", parts);
    }

    private static string GroupToWords(long number)
    {
        if (number == 100)
            return "CIEN";

        string words = Hundreds[number / 100];
        long remainder = number % 100;

        if (remainder > 0)
        {
            string tail = remainder < 30
                ? Units[remainder]
                : remainder % 10 == 0 ? Tens[remainder / 10] : $"{Tens[remainder / 10]} Y {Units[remainder % 10]}";

            words = words.Length > 0 ? $"{words} {tail}" : tail;
        }

        return words;
    }
}
