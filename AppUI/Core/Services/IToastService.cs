using System;
using AppUI.Core.Models;

namespace AppUI.Core.Services;

public readonly record struct ToastMessage(
    string Title,
    string Message,
    Tone Tone = Tone.Blue,
    TimeSpan? Duration = null);

public interface IToastService
{
    event Action<ToastMessage>? OnToastRequested;
    void Show(string title, string message, Tone tone = Tone.Blue, TimeSpan? duration = null);
    void Success(string title, string message);
    void Warning(string title, string message);
    void Danger(string title, string message);
    void Info(string title, string message);
}
