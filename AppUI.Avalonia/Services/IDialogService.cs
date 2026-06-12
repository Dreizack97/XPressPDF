namespace AppUI.Avalonia.Services
{
    public interface IDialogService
    {
        Task<IReadOnlyList<string>> PickXmlFilesAsync();

        Task ShowMessageAsync(string title, string message);

        Task ShowSettingsAsync();
    }
}
