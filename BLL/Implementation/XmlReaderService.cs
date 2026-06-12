using BLL.Interfaces;
using BLL.Utilities;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Schemas.Base;

namespace BLL.Implementation
{
    public class XmlReaderService : IXmlReaderService
    {
        private readonly ComplementService _complementService;
        private readonly XmlDeserializer _xmlDeserializer;

        static XmlReaderService()
        {
            // La licencia se configura una sola vez por proceso, no por archivo procesado.
            Settings.License = LicenseType.Community;
        }

        public XmlReaderService()
        {
            _complementService = new ComplementService();
            _xmlDeserializer = new XmlDeserializer();
        }

        public async Task<bool> Read(string xmlPath)
        {
            if (!File.Exists(xmlPath))
                throw new FileNotFoundException("File not found.", xmlPath);

            string xmlContent = await File.ReadAllTextAsync(xmlPath);

            Comprobante comprobante = _xmlDeserializer.Deserialize<Comprobante>(xmlContent)
                ?? throw new InvalidOperationException($"Failed to deserialize file: {xmlPath}. The XML may be invalid or not match the schema.");

            comprobante = _complementService.GetComplements(comprobante);

            IDocument document = CreateDocument(comprobante);

            string newFileName = string.IsNullOrWhiteSpace(comprobante.TimbreFiscalDigital?.UUID)
                ? Path.ChangeExtension(Path.GetFileName(xmlPath), ".pdf")
                : $"{comprobante.TimbreFiscalDigital.UUID}.pdf";

            string pdfFilePath = Path.Combine(Path.GetDirectoryName(xmlPath)!, newFileName);

            document.GeneratePdf(pdfFilePath);

            return true;
        }

        private static IDocument CreateDocument(Comprobante comprobante) => comprobante switch
        {
            { Nomina: not null } => new PdfGenerator(comprobante),
            { ValesDespensa: not null } => new ValesDespensaDocument(comprobante),
            _ => new InvoiceDocument(comprobante)
        };
    }
}
