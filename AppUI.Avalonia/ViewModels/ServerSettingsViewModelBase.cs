using BLL.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppUI.Avalonia.ViewModels
{
    /// <summary>
    /// Base de las secciones de configuración: comparte el patrón guardar/probar conexión
    /// (validación, estado ocupado y mensaje de resultado) entre FTP y correo.
    /// </summary>
    public abstract partial class ServerSettingsViewModelBase : ViewModelBase
    {
        private readonly IConnectionTestable _connectionTestable;

        protected readonly IConfigManager ConfigManager;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyCanExecuteChangedFor(nameof(TestConnectionCommand))]
        private bool _isBusy;

        [ObservableProperty]
        private string? _resultMessage;

        [ObservableProperty]
        private bool _resultIsError;

        protected ServerSettingsViewModelBase(IConfigManager configManager, IConnectionTestable connectionTestable)
        {
            ConfigManager = configManager;
            _connectionTestable = connectionTestable;
        }

        /// <summary>Valida los campos; devuelve un mensaje de error o null si todo es válido.</summary>
        protected abstract string? Validate();

        /// <summary>Vuelca los campos del formulario a la configuración.</summary>
        protected abstract void ApplyToConfig();

        private bool CanInteract() => !IsBusy;

        [RelayCommand(CanExecute = nameof(CanInteract))]
        private void Save()
        {
            string? validationError = Validate();

            if (validationError != null)
            {
                SetResult(validationError, isError: true);
                return;
            }

            ApplyToConfig();
            ConfigManager.Save(ConfigManager.Current);
            SetResult("La configuración se guardó correctamente.", isError: false);
        }

        [RelayCommand(CanExecute = nameof(CanInteract))]
        private async Task TestConnectionAsync()
        {
            string? validationError = Validate();

            if (validationError != null)
            {
                SetResult(validationError, isError: true);
                return;
            }

            IsBusy = true;

            try
            {
                // Se prueba con los valores del formulario, aunque no se hayan guardado.
                ApplyToConfig();

                bool connected = await _connectionTestable.ConnectAsync();
                SetResult(connected ? "Conexión exitosa." : "No fue posible establecer la conexión.", isError: !connected);
            }
            catch (Exception ex)
            {
                SetResult(ex.Message, isError: true);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetResult(string message, bool isError)
        {
            ResultMessage = message;
            ResultIsError = isError;
        }

        protected static string? RequireFields(params (string Value, string Label)[] fields)
        {
            foreach ((string value, string label) in fields)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return $"El campo \"{label}\" es obligatorio.";
            }

            return null;
        }

        protected static string? ValidatePort(string port) =>
            int.TryParse(port, out int value) && value is > 0 and <= 65535
                ? null
                : "El puerto debe ser un número entre 1 y 65535.";
    }
}
