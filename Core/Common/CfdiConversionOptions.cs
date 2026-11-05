namespace Core.Common;

using System.Globalization;

/// <summary>
/// Opciones configurables para la lectura de XML y renderizado del documento PDF.
/// </summary>
public record CfdiConversionOptions
{
    public string? LogoImagePath { get; init; }

    public byte[]? LogoImageBytes { get; init; }

    public CultureInfo Culture { get; init; } = CultureInfo.GetCultureInfo("es-MX");

    public bool OverwriteExisting { get; init; } = true;

    public string? OutputDirectory { get; init; }
}
