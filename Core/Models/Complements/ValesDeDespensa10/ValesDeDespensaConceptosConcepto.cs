namespace Core.Models.Complements.ValesDeDespensa10;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/valesdedespensa")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/valesdedespensa", IsNullable = false)]
public partial class ValesDeDespensaConceptosConcepto
{
    [XmlAttribute("identificador")]
    public string identificador { get; set; } = string.Empty;

    [XmlAttribute("fecha")]
    public DateTime fecha { get; set; }

    [XmlAttribute("rfc")]
    public string rfc { get; set; } = string.Empty;

    [XmlAttribute("curp")]
    public string curp { get; set; } = string.Empty;

    [XmlAttribute("nombre")]
    public string nombre { get; set; } = string.Empty;

    [XmlAttribute("numSeguridad")]
    public string? numSeguridad { get; set; }

    [XmlAttribute("importe")]
    public decimal importe { get; set; }
}
