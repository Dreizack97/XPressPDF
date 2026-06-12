namespace AppUI.Avalonia.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public FtpSettingsViewModel Ftp { get; }

        public EmailSettingsViewModel Email { get; }

        public SettingsViewModel(FtpSettingsViewModel ftp, EmailSettingsViewModel email)
        {
            Ftp = ftp;
            Email = email;
        }
    }
}
