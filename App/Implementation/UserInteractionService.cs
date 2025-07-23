using App.Interfaces;
using Schemas.Base;

namespace App.Implementation
{
    public class UserInteractionService : IUserInteractionService
    {
        public string RequestFilePath()
        {
            Console.Write("Enter the XML file path: ");
            return Console.ReadLine();
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
            Console.WriteLine($"Invoice Issuer: {comprobante.Emisor?.Nombre}");
            Console.WriteLine($"Receiver: {comprobante.Receptor?.Nombre}");
            Console.WriteLine($"Date: {comprobante.Fecha}");
            Console.WriteLine($"Total: {comprobante.Total}");
            Console.WriteLine($"UUID: {comprobante.TimbreFiscalDigital?.UUID ?? ""}");
            // etc...
        }
    }
}
