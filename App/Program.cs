using App.Implementation;
using App.Utilities;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Schemas.Base;
using System.Globalization;

internal class Program
{
    private static async Task Main(string[] args)
    {
        FileService fileService = new FileService();
        XmlDeserializer deserializer = new XmlDeserializer();
        UserInteractionService uiService = new UserInteractionService();

        while (true)
        {
            int option = 0;
            List<string> files = new List<string>();

            Console.Clear();
            Console.WriteLine("===== XPressPDF =====");

            do
            {
                uiService.ShowMenu();
            }
            while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 3);

            switch (option)
            {
                case 1:
                    string filePath = uiService.RequestFilePath().Trim('"');
                    if (string.IsNullOrWhiteSpace(filePath))
                    {
                        uiService.ShowError("\nFile name cannot be empty.");
                        break;
                    }
                    if (!fileService.FileExists(filePath))
                        uiService.ShowError($"\nFile not found: {filePath}");
                    else
                        files.Add(filePath);
                    break;
                case 2:
                    string directoryPath = uiService.RequestDirectoryPath().Trim('"');
                    if (string.IsNullOrWhiteSpace(directoryPath))
                    {
                        uiService.ShowError("\nDirectory path cannot be empty.");
                        break;
                    }
                    if (!fileService.DirectoryExists(directoryPath))
                        uiService.ShowError($"\nDirectory not found: {directoryPath}");
                    else
                    {
                        var xmlFiles = Directory.GetFiles(directoryPath, "*.xml");
                        if (!xmlFiles.Any())
                            uiService.ShowError("\nNo XML files found in the specified directory.");
                        else
                            files.AddRange(xmlFiles);
                    }
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }

            if (!files.Any())
            {
                Console.Write("\nNo files were processed. Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            try
            {
                foreach (string filePath in files)
                {
                    string xmlContent = await fileService.ReadFileAsync(filePath);
                    Comprobante? comprobante = deserializer.Deserialize<Comprobante>(xmlContent);

                    if (comprobante == null)
                    {
                        uiService.ShowError($"\nFailed to deserialize file: {filePath}. The XML may be invalid or not match the schema.");
                        continue;
                    }

                    ComplementService complementService = new ComplementService();
                    comprobante = complementService.GetComplements(comprobante);

                    if (option == 1)
                        uiService.ShowInvoiceSummary(comprobante);
                    else
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        Settings.License = LicenseType.Community;
                        PdfGenerator document = new PdfGenerator(comprobante);
                        document.GeneratePdf(fileInfo.FullName.Replace(".xml", ".pdf", true, CultureInfo.InvariantCulture));
                    }
                }

                uiService.ShowSuccess("\nXML processed successfully!");
            }
            catch (Exception ex)
            {
                uiService.ShowError($"Error: {ex.Message}");
            }

            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
