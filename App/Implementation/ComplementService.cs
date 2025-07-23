using App.Interfaces;
using Schemas.Base;
using Schemas.Complements;
using System.Xml;

namespace App.Implementation
{
    public class ComplementService : IComplementService
    {
        private readonly XmlDeserializer _invoiceDeserializer = new XmlDeserializer();

        public Comprobante GetComplements(Comprobante comprobante)
        {
            if (comprobante?.Complemento?.Any != null)
            {
                foreach (XmlElement element in comprobante.Complemento.Any)
                {
                    switch (element.LocalName)
                    {
                        case "TimbreFiscalDigital":
                            comprobante.TimbreFiscalDigital = _invoiceDeserializer.Deserialize<TimbreFiscalDigital>(element.OuterXml);
                            break;
                    }
                }
            }

            return comprobante;
        }
    }
}
