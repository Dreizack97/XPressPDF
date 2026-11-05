namespace AppUI.Core.Models;

/// <summary>
/// Preferencias de usuario y configuración del motor PDF persistidas localmente en JSON.
/// </summary>
public sealed class AppPreferences
{
    /// <summary>
    /// Tema visual seleccionado: "System", "Light" o "Dark".
    /// </summary>
    public string SelectedTheme { get; set; } = "System";

    /// <summary>
    /// Carpeta de salida personalizada para los PDFs. Si es null o vacía, se guardan junto al XML original.
    /// </summary>
    public string? DefaultOutputDirectory { get; set; }

    /// <summary>
    /// Ruta del archivo de logotipo corporativo (.png, .jpg, .webp).
    /// </summary>
    public string? LogoPath { get; set; }

    /// <summary>
    /// Si es true, sobreescribe los PDFs con el mismo nombre si ya existen en el destino.
    /// </summary>
    public bool OverwriteExisting { get; set; } = true;

    /// <summary>
    /// Si es true, analiza metadatos fiscales básicos (UUID, Tipo, Emisor, Receptor, Total) al agregar los XML a la cola.
    /// </summary>
    public bool PreScanMetadata { get; set; } = true;

    /// <summary>
    /// Si es true, desactiva animaciones y transiciones visuales (accesibilidad).
    /// </summary>
    public bool ReducedMotion { get; set; } = false;
}
