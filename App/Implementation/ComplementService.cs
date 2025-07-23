using App.Interfaces;
using Schemas;
using Schemas.Base;
using Schemas.Complements;
using System.Xml;

namespace App.Implementation
{
    public class ComplementService : IComplementService
    {
        private readonly XmlDeserializer _invoiceDeserializer = new XmlDeserializer();

        public IEnumerable<IComplement> GetComplements(Comprobante comprobante)
        {
            List<IComplement> complements = new List<IComplement>();

            if (comprobante.Complemento == null)
                return complements;

            foreach (XmlElement element in comprobante.Complemento.Any)
            {
                switch (element.LocalName)
                {
                    case "TimbreFiscalDigital":
                        TimbreFiscalDigital? timbre = _invoiceDeserializer.Deserialize<TimbreFiscalDigital>(element.OuterXml);

                        if (timbre != null)
                            complements.Add(timbre);
                        break;
                }
            }

            return complements;
        }
    }
}
