namespace BLL.Objects
{
    public class MailServerConfig
    {
        public string Address { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string Host { get; set; } = null!;

        public int Port { get; set; } = 587;

        public bool SSL { get; set; } = true;
    }
}
