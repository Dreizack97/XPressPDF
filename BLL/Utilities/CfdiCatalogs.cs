namespace BLL.Utilities
{
    /// <summary>
    /// Descripciones de los catálogos del SAT requeridos en la representación impresa de un CFDI.
    /// </summary>
    public static class CfdiCatalogs
    {
        private static readonly Dictionary<string, string> FormaPago = new()
        {
            ["01"] = "Efectivo",
            ["02"] = "Cheque nominativo",
            ["03"] = "Transferencia electrónica de fondos",
            ["04"] = "Tarjeta de crédito",
            ["05"] = "Monedero electrónico",
            ["06"] = "Dinero electrónico",
            ["08"] = "Vales de despensa",
            ["12"] = "Dación en pago",
            ["13"] = "Pago por subrogación",
            ["14"] = "Pago por consignación",
            ["15"] = "Condonación",
            ["17"] = "Compensación",
            ["23"] = "Novación",
            ["24"] = "Confusión",
            ["25"] = "Remisión de deuda",
            ["26"] = "Prescripción o caducidad",
            ["27"] = "A satisfacción del acreedor",
            ["28"] = "Tarjeta de débito",
            ["29"] = "Tarjeta de servicios",
            ["30"] = "Aplicación de anticipos",
            ["31"] = "Intermediario pagos",
            ["99"] = "Por definir"
        };

        private static readonly Dictionary<string, string> MetodoPago = new()
        {
            ["PUE"] = "Pago en una sola exhibición",
            ["PPD"] = "Pago en parcialidades o diferido"
        };

        private static readonly Dictionary<string, string> UsoCfdi = new()
        {
            ["G01"] = "Adquisición de mercancías",
            ["G02"] = "Devoluciones, descuentos o bonificaciones",
            ["G03"] = "Gastos en general",
            ["I01"] = "Construcciones",
            ["I02"] = "Mobiliario y equipo de oficina por inversiones",
            ["I03"] = "Equipo de transporte",
            ["I04"] = "Equipo de cómputo y accesorios",
            ["I05"] = "Dados, troqueles, moldes, matrices y herramental",
            ["I06"] = "Comunicaciones telefónicas",
            ["I07"] = "Comunicaciones satelitales",
            ["I08"] = "Otra maquinaria y equipo",
            ["D01"] = "Honorarios médicos, dentales y gastos hospitalarios",
            ["D02"] = "Gastos médicos por incapacidad o discapacidad",
            ["D03"] = "Gastos funerales",
            ["D04"] = "Donativos",
            ["D05"] = "Intereses reales efectivamente pagados por créditos hipotecarios",
            ["D06"] = "Aportaciones voluntarias al SAR",
            ["D07"] = "Primas por seguros de gastos médicos",
            ["D08"] = "Gastos de transportación escolar obligatoria",
            ["D09"] = "Depósitos en cuentas para el ahorro, primas de pensiones",
            ["D10"] = "Pagos por servicios educativos (colegiaturas)",
            ["S01"] = "Sin efectos fiscales",
            ["CP01"] = "Pagos",
            ["CN01"] = "Nómina"
        };

        private static readonly Dictionary<int, string> RegimenFiscal = new()
        {
            [601] = "General de Ley Personas Morales",
            [603] = "Personas Morales con Fines no Lucrativos",
            [605] = "Sueldos y Salarios e Ingresos Asimilados a Salarios",
            [606] = "Arrendamiento",
            [607] = "Régimen de Enajenación o Adquisición de Bienes",
            [608] = "Demás ingresos",
            [610] = "Residentes en el Extranjero sin Establecimiento Permanente en México",
            [611] = "Ingresos por Dividendos (socios y accionistas)",
            [612] = "Personas Físicas con Actividades Empresariales y Profesionales",
            [614] = "Ingresos por intereses",
            [615] = "Régimen de los ingresos por obtención de premios",
            [616] = "Sin obligaciones fiscales",
            [620] = "Sociedades Cooperativas de Producción que optan por diferir sus ingresos",
            [621] = "Incorporación Fiscal",
            [622] = "Actividades Agrícolas, Ganaderas, Silvícolas y Pesqueras",
            [623] = "Opcional para Grupos de Sociedades",
            [624] = "Coordinados",
            [625] = "Régimen de las Actividades Empresariales con ingresos a través de Plataformas Tecnológicas",
            [626] = "Régimen Simplificado de Confianza"
        };

        private static readonly Dictionary<string, string> TipoComprobante = new()
        {
            ["I"] = "Ingreso",
            ["E"] = "Egreso",
            ["T"] = "Traslado",
            ["N"] = "Nómina",
            ["P"] = "Pago"
        };

        private static readonly Dictionary<string, string> Impuesto = new()
        {
            ["001"] = "ISR",
            ["002"] = "IVA",
            ["003"] = "IEPS"
        };

        private static readonly Dictionary<string, string> Exportacion = new()
        {
            ["01"] = "No aplica",
            ["02"] = "Definitiva",
            ["03"] = "Temporal",
            ["04"] = "Definitiva con clave distinta a A1"
        };

        public static string GetFormaPago(string? code) => Describe(FormaPago, code);

        public static string GetMetodoPago(string? code) => Describe(MetodoPago, code);

        public static string GetUsoCfdi(string? code) => Describe(UsoCfdi, code);

        public static string GetRegimenFiscal(int code) => RegimenFiscal.TryGetValue(code, out string? description) ? $"{code} - {description}" : code.ToString();

        public static string GetTipoComprobante(string? code) => Describe(TipoComprobante, code);

        /// <summary>Devuelve solo la descripción del tipo de comprobante, sin el código.</summary>
        public static string GetTipoComprobanteName(string? code) =>
            code != null && TipoComprobante.TryGetValue(code, out string? description) ? description : code ?? string.Empty;

        public static string GetImpuesto(string? code) => Describe(Impuesto, code);

        public static string GetExportacion(string? code) => Describe(Exportacion, code);

        private static string Describe(Dictionary<string, string> catalog, string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return string.Empty;

            return catalog.TryGetValue(code, out string? description) ? $"{code} - {description}" : code;
        }
    }
}
