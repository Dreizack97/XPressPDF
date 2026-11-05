using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppUI.Core.Icons;
using AppUI.Core.Models;
using AppUI.Core.Services;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppUI.Shared.Controls.Feedback;

public partial class ToastItemViewModel : ObservableObject
{
    public string Title { get; }
    public string Message { get; }
    public Tone Tone { get; }
    public LucideIconKind IconKind { get; }

    public ToastItemViewModel(ToastMessage msg)
    {
        Title = msg.Title;
        Message = msg.Message;
        Tone = msg.Tone;
        IconKind = msg.Tone switch
        {
            Tone.Green => LucideIconKind.CircleCheck,
            Tone.Yellow => LucideIconKind.CircleAlert,
            Tone.Red => LucideIconKind.CircleAlert,
            _ => LucideIconKind.Info
        };
    }
}

public partial class ErpToastHost : UserControl
{
    public ObservableCollection<ToastItemViewModel> Toasts { get; } = new();

    public ErpToastHost()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void AttachToastService(IToastService toastService)
    {
        toastService.OnToastRequested += OnToastRequested;
    }

    private void OnToastRequested(ToastMessage message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var item = new ToastItemViewModel(message);
            Toasts.Add(item);

            var duration = message.Duration ?? TimeSpan.FromSeconds(3.5);
            _ = DismissAfterDelayAsync(item, duration);
        });
    }

    [RelayCommand]
    private void Dismiss(ToastItemViewModel item)
    {
        Toasts.Remove(item);
    }

    private async Task DismissAfterDelayAsync(ToastItemViewModel item, TimeSpan delay)
    {
        await Task.Delay(delay);
        Dispatcher.UIThread.Post(() => Toasts.Remove(item));
    }
}
