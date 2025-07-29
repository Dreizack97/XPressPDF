namespace BLL.Objects
{
    public class FtpServerConfig
    {
        public string Host { get; set; } = null!;

        public string User { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int Port { get; set; } = 21;

        public string RootPath { get; set; } = null!;
    }
}
