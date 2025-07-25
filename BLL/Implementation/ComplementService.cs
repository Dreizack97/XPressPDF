﻿using BLL.Interfaces;
using Schemas.Base;
using Schemas.Complements;
using Schemas.Complements.Nomina;
using Schemas.Complements.ValesDeDespensa;
using System.Xml;

namespace BLL.Implementation
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
                        case "Nomina":
                            comprobante.Nomina = _invoiceDeserializer.Deserialize<Nomina>(element.OuterXml);
                            break;
                        case "ValesDeDespensa":
                            comprobante.ValesDespensa = _invoiceDeserializer.Deserialize<ValesDeDespensa>(element.OuterXml);
                            break;
                    }
                }
            }

            return comprobante;
        }
    }
}
