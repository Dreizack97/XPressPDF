namespace Core.Models.Complements.Nomina12;

using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public enum NominaReceptorSindicalizado
{
    Sí,
    No
}
