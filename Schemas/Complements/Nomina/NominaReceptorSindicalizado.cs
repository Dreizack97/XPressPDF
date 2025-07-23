using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public enum NominaReceptorSindicalizado
    {
        Sí,

        No,
    }
}
