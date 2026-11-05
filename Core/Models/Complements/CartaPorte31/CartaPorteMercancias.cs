namespace Core.Models.Complements.CartaPorte31;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteMercancias
{
    [XmlAttribute]
    public decimal PesoBrutoTotal { get; set; }

    [XmlAttribute]
    public string UnidadPeso { get; set; } = "KGM";

    [XmlAttribute]
    public decimal PesoNetoTotal { get; set; }

    [XmlIgnore]
    public bool PesoNetoTotalSpecified { get; set; }

    [XmlAttribute]
    public int NumTotalMercancias { get; set; }

    [XmlElement("Mercancia")]
    public List<CartaPorteMercancia> Mercancia { get; set; } = [];

    [XmlElement]
    public CartaPorteAutotransporte? Autotransporte { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteMercancia
{
    [XmlAttribute]
    public string BienesTransp { get; set; } = string.Empty;

    [XmlAttribute]
    public string? ClaveSTCC { get; set; }

    [XmlAttribute]
    public string Descripcion { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal Cantidad { get; set; }

    [XmlAttribute]
    public string ClaveUnidad { get; set; } = string.Empty;

    [XmlAttribute]
    public string? Unidad { get; set; }

    [XmlAttribute]
    public string? Dimensiones { get; set; }

    [XmlAttribute]
    public string? MaterialPeligroso { get; set; }

    [XmlAttribute]
    public string? CveMaterialPeligroso { get; set; }

    [XmlAttribute]
    public string? Embalaje { get; set; }

    [XmlAttribute]
    public string? DescripEmbalaje { get; set; }

    [XmlAttribute]
    public decimal PesoEnKg { get; set; }

    [XmlAttribute]
    public decimal ValorMercancia { get; set; }

    [XmlIgnore]
    public bool ValorMercanciaSpecified { get; set; }

    [XmlAttribute]
    public string? Moneda { get; set; }

    [XmlAttribute]
    public string? FraccionArancelaria { get; set; }

    [XmlAttribute]
    public string? UUIDComercioExt { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteAutotransporte
{
    [XmlAttribute]
    public string PermSCT { get; set; } = string.Empty;

    [XmlAttribute]
    public string NumPermisoSCT { get; set; } = string.Empty;

    [XmlElement]
    public CartaPorteIdentificacionVehicular? IdentificacionVehicular { get; set; }

    [XmlElement]
    public CartaPorteSeguros? Seguros { get; set; }

    [XmlArrayItem("Remolque", IsNullable = false)]
    public List<CartaPorteRemolque> Remolques { get; set; } = [];
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteIdentificacionVehicular
{
    [XmlAttribute]
    public string ConfigVehicular { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal PesoBrutoVehicular { get; set; }

    [XmlAttribute]
    public string PlacaVM { get; set; } = string.Empty;

    [XmlAttribute]
    public int AnioModeloVM { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteSeguros
{
    [XmlAttribute]
    public string AseguraRespCivil { get; set; } = string.Empty;

    [XmlAttribute]
    public string PolizaRespCivil { get; set; } = string.Empty;

    [XmlAttribute]
    public string? AseguraMedAmbiente { get; set; }

    [XmlAttribute]
    public string? PolizaMedAmbiente { get; set; }

    [XmlAttribute]
    public string? AseguraCarga { get; set; }

    [XmlAttribute]
    public string? PolizaCarga { get; set; }

    [XmlAttribute]
    public decimal Prima { get; set; }

    [XmlIgnore]
    public bool PrimaSpecified { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteRemolque
{
    [XmlAttribute]
    public string SubTipoRem { get; set; } = string.Empty;

    [XmlAttribute]
    public string Placa { get; set; } = string.Empty;
}
