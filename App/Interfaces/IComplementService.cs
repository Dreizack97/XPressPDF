using Schemas;
using Schemas.Base;

namespace App.Interfaces
{
    public interface IComplementService
    {
        IEnumerable<IComplement> GetComplements(Comprobante comprobante);
    }
}
