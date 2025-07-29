using BLL.Objects;
using System.Text.Json;

namespace AppUI.Utilities
{
    public static class ConfigManager
    {
        public static void CreateDefaultConfig(string configPath)
        {
            AppConfig defaultConfig = new AppConfig()
            {
                FtpServer = new FtpServerConfig
                {
                    Host = "",
                    User = "",
                    Password = "",
                    Port = 21,
                    RootPath = ""
                },
                MailServer = new MailServerConfig
                {
                    Address = "",
                    Password = "",
                    DisplayName = "",
                    Host = "",
                    Port = 587,
                    SSL = true
                }
            };

            string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, json);
        }
    }
}
