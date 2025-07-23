using App.Implementation;
using Schemas.Base;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("===== XML Invoice Reader (.NET 9) =====");

        FileService fileService = new FileService();
        XmlDeserializer deserializer = new XmlDeserializer();
        UserInteractionService uiService = new UserInteractionService();

        string filePath = uiService.RequestFilePath();

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

            uiService.ShowSuccess("XML deserialized successfully!");
            uiService.ShowInvoiceSummary(comprobante);
        }
        catch (Exception ex)
        {
            uiService.ShowError($"Error: {ex.Message}");
        }

        Console.ReadLine();
    }
}