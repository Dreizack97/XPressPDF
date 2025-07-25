using BLL.Implementation;
using BLL.Utilities;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Schemas.Base;
using System.Globalization;

namespace BLL
{
    public class XmlReader
    {
        private readonly ComplementService _complementService;
        private readonly XmlDeserializer _xmlDeserializer;

        public XmlReader()
        {
            _complementService = new ComplementService();
            _xmlDeserializer = new XmlDeserializer();
        }

        public async Task Read(string xmlPath)
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

                document.GeneratePdf(fileInfo.FullName.Replace(".xml", ".pdf", true, CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
