namespace AppUI.Core.Models;

/// <summary>
/// Estados semánticos de negocio del sistema de diseño (paridad con StatusTone en Smart-Enterprise-ERP-Design-System).
/// Cada estado se mapea a un Tone cromático específico.
/// </summary>
public enum StatusTone
{
    Success,
    Warning,
    Danger,
    Neutral,
    Info
}

public static class StatusToneExtensions
{
    /// <summary>
    /// Resuelve el tono cromático correspondiente al estado semántico.
    /// </summary>
    public static Tone ToTone(this StatusTone status) => status switch
    {
        StatusTone.Success => Tone.Green,
        StatusTone.Warning => Tone.Yellow,
        StatusTone.Danger => Tone.Red,
        StatusTone.Neutral => Tone.Neutral,
        StatusTone.Info => Tone.Blue,
        _ => Tone.Neutral
    };
}
