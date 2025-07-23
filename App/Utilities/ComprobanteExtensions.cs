using App.Interfaces;
using Schemas;
using Schemas.Base;

namespace App.Utilities
{
    public static class ComprobanteExtensions
    {
        public static IEnumerable<IComplement> GetAllComplements(this Comprobante comprobante, IComplementService service)
        {
            return service.GetComplements(comprobante);
        }
    }
}
