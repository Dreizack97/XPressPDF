namespace Core.Models.Complements.CartaPorte31;

using System;
using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteUbicacion
{
    [XmlAttribute]
    public string TipoUbicacion { get; set; } = string.Empty;

    [XmlAttribute]
    public string? IdUbicacion { get; set; }

    [XmlAttribute]
    public string RfcRemitenteDestinatario { get; set; } = string.Empty;

    [XmlAttribute]
    public string? NombreRemitenteDestinatario { get; set; }

    [XmlAttribute]
    public string? NumRegIdTrib { get; set; }

    [XmlAttribute]
    public string? ResidenciaFiscal { get; set; }

    [XmlAttribute]
    public string? NumEstacion { get; set; }

    [XmlAttribute]
    public string? NombreEstacion { get; set; }

    [XmlAttribute]
    public string? NavegacionTrafico { get; set; }

    [XmlAttribute]
    public DateTime FechaHoraSalidaLlegada { get; set; }

    [XmlAttribute]
    public string? TipoEstacion { get; set; }

    [XmlAttribute]
    public decimal DistanciaRecorrida { get; set; }

    [XmlIgnore]
    public bool DistanciaRecorridaSpecified { get; set; }

    [XmlElement]
    public CartaPorteDomicilio? Domicilio { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteDomicilio
{
    [XmlAttribute]
    public string? Calle { get; set; }

    [XmlAttribute]
    public string? NumeroExterior { get; set; }

    [XmlAttribute]
    public string? NumeroInterior { get; set; }

    [XmlAttribute]
    public string? Colonia { get; set; }

    [XmlAttribute]
    public string? Localidad { get; set; }

    [XmlAttribute]
    public string? Referencia { get; set; }

    [XmlAttribute]
    public string? Municipio { get; set; }

    [XmlAttribute]
    public string Estado { get; set; } = string.Empty;

    [XmlAttribute]
    public string Pais { get; set; } = "MEX";

    [XmlAttribute]
    public string CodigoPostal { get; set; } = string.Empty;
}
