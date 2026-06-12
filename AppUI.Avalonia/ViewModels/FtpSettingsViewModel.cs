using BLL.Interfaces;
using BLL.Objects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppUI.Avalonia.ViewModels
{
    public partial class FtpSettingsViewModel : ServerSettingsViewModelBase
    {
        [ObservableProperty]
        private string _host = string.Empty;

        [ObservableProperty]
        private string _user = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _port = string.Empty;

        [ObservableProperty]
        private string _rootPath = string.Empty;

        public FtpSettingsViewModel(IConfigManager configManager, IFtpService ftpService)
            : base(configManager, ftpService)
        {
            FtpServerConfig config = configManager.Current.FtpServer;

            Host = config.Host;
            User = config.User;
            Password = config.Password;
            Port = config.Port.ToString();
            RootPath = config.RootPath;
        }

        protected override string? Validate() =>
            RequireFields((Host, "Servidor"), (User, "Usuario"), (Password, "Contraseña"), (Port, "Puerto"), (RootPath, "Ruta raíz"))
                ?? ValidatePort(Port);

        protected override void ApplyToConfig()
        {
            FtpServerConfig config = ConfigManager.Current.FtpServer;

            config.Host = Host;
            config.User = User;
            config.Password = Password;
            config.Port = int.Parse(Port);
            config.RootPath = RootPath;
        }
    }
}
