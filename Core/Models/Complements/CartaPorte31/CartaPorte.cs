namespace Core.Models.Complements.CartaPorte31;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/CartaPorte31", IsNullable = false)]
public sealed class CartaPorte : ICfdiComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/CartaPorte31";

    [XmlIgnore]
    public string ComplementName => "Carta Porte 3.1";

    [XmlAttribute]
    public string Version { get; set; } = "3.1";

    [XmlAttribute]
    public string IdCCP { get; set; } = string.Empty;

    [XmlAttribute]
    public string TranspInternac { get; set; } = "No";

    [XmlAttribute]
    public string? RegimenAduanero { get; set; }

    [XmlAttribute]
    public string? EntradaSalidaMerc { get; set; }

    [XmlAttribute]
    public string? PaisOrigenDestino { get; set; }

    [XmlAttribute]
    public string? ViaEntradaSalida { get; set; }

    [XmlAttribute]
    public decimal TotalDistRec { get; set; }

    [XmlIgnore]
    public bool TotalDistRecSpecified { get; set; }

    [XmlAttribute]
    public string? RegistroISTMO { get; set; }

    [XmlAttribute]
    public string? UbicacionPoloOrigen { get; set; }

    [XmlAttribute]
    public string? UbicacionPoloDestino { get; set; }

    [XmlArrayItem("Ubicacion", IsNullable = false)]
    public List<CartaPorteUbicacion> Ubicaciones { get; set; } = [];

    [XmlElement]
    public CartaPorteMercancias? Mercancias { get; set; }

    [XmlArrayItem("TiposFigura", IsNullable = false)]
    public List<CartaPorteTiposFigura> FiguraTransporte { get; set; } = [];
}
