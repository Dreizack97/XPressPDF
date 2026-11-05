namespace Core.Abstractions;

using Core.Models.Base;
using QuestPDF.Fluent;

/// <summary>
/// Estrategia desacoplada para renderizar la sección de un complemento en el documento PDF.
/// </summary>
public interface ICfdiComplementSectionRenderer
{
    int Order => 100;

    bool CanRender(Comprobante comprobante);

    void Render(ColumnDescriptor column, Comprobante comprobante);
}
