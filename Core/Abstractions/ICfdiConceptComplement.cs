namespace Core.Abstractions;

/// <summary>
/// Contrato base para los complementos aplicables a un concepto del CFDI (&lt;cfdi:ComplementoConcepto&gt;).
/// </summary>
public interface ICfdiConceptComplement
{
    string ConceptComplementName { get; }
}
