using Schemas.Base;

namespace App.Interfaces
{
    internal interface IUserInteractionService
    {
        string RequestFilePath();

        string RequestDirectoryPath();

        void ShowMenu();

        void ShowError(string message);

        void ShowSuccess(string message);

        void ShowInvoiceSummary(Comprobante comprobante);
    }
}
