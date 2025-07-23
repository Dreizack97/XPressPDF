using Schemas.Base;

namespace App.Interfaces
{
    internal interface IUserInteractionService
    {
        string RequestFilePath();

        void ShowError(string message);

        void ShowSuccess(string message);

        void ShowInvoiceSummary(Comprobante comprobante);
    }
}
