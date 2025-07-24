using App.Implementation;
using App.Utilities;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Schemas.Base;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("===== XPressPDF =====");

        FileService fileService = new FileService();
        XmlDeserializer deserializer = new XmlDeserializer();
        UserInteractionService uiService = new UserInteractionService();

        string filePath = uiService.RequestFilePath().Trim('"');

        if (!fileService.FileExists(filePath))
        {
            uiService.ShowError($"File not found: {filePath}");
            return;
        }

        try
        {
            string xmlContent = await fileService.ReadFileAsync(filePath);
            Comprobante? comprobante = deserializer.Deserialize<Comprobante>(xmlContent);

            if (comprobante == null)
            {
                uiService.ShowError("Could not deserialize the XML file. The file may be invalid or does not match the schema.");
                return;
            }

            ComplementService complementService = new ComplementService();
            comprobante = complementService.GetComplements(comprobante);

            uiService.ShowSuccess("XML deserialized successfully!");
            uiService.ShowInvoiceSummary(comprobante);

            Settings.License = LicenseType.Community;
            PdfGenerator document = new PdfGenerator(comprobante);
            document.GeneratePdf(filePath.ToUpper().Replace(".XML", ".pdf"));
        }
        catch (Exception ex)
        {
            uiService.ShowError($"Error: {ex.Message}");
        }

        Console.ReadLine();
    }
}