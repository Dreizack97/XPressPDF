using BLL.Interfaces;
using BLL.Utilities;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Schemas.Base;
using System.Diagnostics;
using System.Globalization;

namespace BLL.Implementation
{
    public class XmlReaderService : IXmlReaderService
    {
        private readonly ComplementService _complementService;
        private readonly XmlDeserializer _xmlDeserializer;

        public XmlReaderService()
        {
            _complementService = new ComplementService();
            _xmlDeserializer = new XmlDeserializer();
        }

        public async Task<bool> Read(string xmlPath)
        {
            if (!File.Exists(xmlPath))
                throw new FileNotFoundException("File not found.", xmlPath);

            try
            {
                string xmlContent = await File.ReadAllTextAsync(xmlPath);

                Comprobante? comprobante = _xmlDeserializer.Deserialize<Comprobante>(xmlContent)
                    ?? throw new InvalidOperationException($"Failed to deserialize file: {xmlPath}. The XML may be invalid or not match the schema.");

                comprobante = _complementService.GetComplements(comprobante);

                FileInfo fileInfo = new FileInfo(xmlPath);

                Settings.License = LicenseType.Community;
                PdfGenerator document = new PdfGenerator(comprobante);

                string newFileName = $"{comprobante.TimbreFiscalDigital?.UUID}.pdf" ?? fileInfo.Name.Replace(".xml", ".pdf", true, CultureInfo.InvariantCulture);
                string pdfFilePath = Path.Combine(fileInfo.DirectoryName!, newFileName);

                document.GeneratePdf(pdfFilePath);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
