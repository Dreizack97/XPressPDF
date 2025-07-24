using App.Interfaces;
using Schemas.Base;

namespace App.Implementation
{
    public class UserInteractionService : IUserInteractionService
    {
        public string RequestFilePath()
        {
            Console.Write("\nEnter the XML file path: ");
            return Console.ReadLine();
        }

        public string RequestDirectoryPath()
        {
            Console.Write("\nEnter the directory files path: ");
            return Console.ReadLine();
        }

        public void ShowMenu()
        {
            Console.WriteLine("\n1. Enter XML File");
            Console.WriteLine("2. Select Directory");
            Console.WriteLine("3. Exit\n");
            Console.Write("Choose an option: ");
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void ShowInvoiceSummary(Comprobante comprobante)
        {
            // This method should be expanded based on your 'Comprobante' class structure
            Console.WriteLine($"\nInvoice Issuer: {comprobante.Emisor?.Nombre}");
            Console.WriteLine($"Receiver: {comprobante.Receptor?.Nombre}");
            Console.WriteLine($"Date: {comprobante.Fecha}");
            Console.WriteLine($"Total: {comprobante.Total}");
            Console.WriteLine($"UUID: {comprobante.TimbreFiscalDigital?.UUID ?? ""}");
            Console.WriteLine($"Total Percepciones: {comprobante.Nomina?.TotalPercepciones.ToString() ?? ""}");
            Console.WriteLine($"Total Vales: {comprobante.ValesDespensa?.Total.ToString() ?? ""}");
            // etc...
        }
    }
}
