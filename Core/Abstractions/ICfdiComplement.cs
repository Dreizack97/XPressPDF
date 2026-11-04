namespace Core.Abstractions;

/// <summary>
/// Contrato base para los complementos de factura del SAT (&lt;cfdi:Complemento&gt;).
/// </summary>
public interface ICfdiComplement
{
    string ComplementName { get; }
}
