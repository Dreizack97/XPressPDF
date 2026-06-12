using BLL.Interfaces;
using BLL.Objects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.Avalonia.ViewModels
{
    public partial class EmailSettingsViewModel : ServerSettingsViewModelBase
    {
        [ObservableProperty]
        private string _address = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _displayName = string.Empty;

        [ObservableProperty]
        private string _host = string.Empty;

        [ObservableProperty]
        private string _port = string.Empty;

        [ObservableProperty]
        private bool _ssl = true;

        public EmailSettingsViewModel(IConfigManager configManager, IEmailService emailService)
            : base(configManager, emailService)
        {
            MailServerConfig config = configManager.Current.MailServer;

            Address = config.Address;
            Password = config.Password;
            DisplayName = config.DisplayName;
            Host = config.Host;
            Port = config.Port.ToString();
            Ssl = config.SSL;
        }

        protected override string? Validate() =>
            RequireFields((Address, "Correo"), (Password, "Contraseña"), (DisplayName, "Nombre para mostrar"), (Host, "Servidor"), (Port, "Puerto"))
                ?? ValidatePort(Port);

        protected override void ApplyToConfig()
        {
            MailServerConfig config = ConfigManager.Current.MailServer;

            config.Address = Address;
            config.Password = Password;
            config.DisplayName = DisplayName;
            config.Host = Host;
            config.Port = int.Parse(Port);
            config.SSL = Ssl;
        }
    }
}
