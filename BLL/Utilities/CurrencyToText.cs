namespace BLL.Utilities
{
    /// <summary>
    /// Convierte un importe a su representación con letra conforme al formato usado en la
    /// representación impresa de un CFDI, p. ej. "(MIL DOSCIENTOS PESOS 50/100 M.N.)".
    /// </summary>
    public static class CurrencyToText
    {
        private static readonly string[] Units =
        {
            "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE", "DIEZ",
            "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISÉIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE",
            "VEINTE", "VEINTIÚN", "VEINTIDÓS", "VEINTITRÉS", "VEINTICUATRO", "VEINTICINCO",
            "VEINTISÉIS", "VEINTISIETE", "VEINTIOCHO", "VEINTINUEVE"
        };

        private static readonly string[] Tens = { "", "", "", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };

        private static readonly string[] Hundreds = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

        public static string ToText(decimal amount, string? currency = "MXN")
        {
            long integerPart = (long)Math.Truncate(amount);
            int cents = (int)Math.Round((amount - integerPart) * 100, MidpointRounding.AwayFromZero);

            if (cents == 100)
            {
                integerPart++;
                cents = 0;
            }

            string currencyName = currency switch
            {
                null or "" or "MXN" => integerPart == 1 ? "PESO" : "PESOS",
                "USD" => integerPart == 1 ? "DÓLAR" : "DÓLARES",
                "EUR" => integerPart == 1 ? "EURO" : "EUROS",
                _ => currency
            };

            string suffix = currency is null or "" or "MXN" ? "M.N." : "M.E.";

            return $"({ToWords(integerPart)} {currencyName} {cents:00}/100 {suffix})";
        }

        private static string ToWords(long number)
        {
            if (number == 0)
                return "CERO";

            List<string> parts = new List<string>();

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

            return string.Join(' ', parts);
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
}
