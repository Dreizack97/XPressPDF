using System;
using AppUI.Core.Models;

namespace AppUI.Core.Services;

public sealed class ToastService : IToastService
{
    public event Action<ToastMessage>? OnToastRequested;

    public void Show(string title, string message, Tone tone = Tone.Blue, TimeSpan? duration = null)
    {
        OnToastRequested?.Invoke(new ToastMessage(title, message, tone, duration));
    }

    public void Success(string title, string message) =>
        Show(title, message, Tone.Green, TimeSpan.FromSeconds(3.5));

    public void Warning(string title, string message) =>
        Show(title, message, Tone.Yellow, TimeSpan.FromSeconds(4.5));

    public void Danger(string title, string message) =>
        Show(title, message, Tone.Red, TimeSpan.FromSeconds(6.0));

    public void Info(string title, string message) =>
        Show(title, message, Tone.Blue, TimeSpan.FromSeconds(3.5));
}
