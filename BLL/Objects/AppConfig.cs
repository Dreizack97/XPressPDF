namespace BLL.Objects
{
    public class AppConfig
    {
        public FtpServerConfig FtpServer { get; set; } = null!;

        public MailServerConfig MailServer { get; set; } = null!;
    }
}
