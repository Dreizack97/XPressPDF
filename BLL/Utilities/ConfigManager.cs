using BLL.Objects;
using System.Text.Json;

namespace BLL.Utilities
{
    public static class ConfigManager
    {
        private static readonly string CONFIG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static void CreateDefaultConfig()
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
            File.WriteAllText(CONFIG_PATH, json);
        }

        public static AppConfig LoadConfig()
        {
            string json = File.ReadAllText(CONFIG_PATH);

            return JsonSerializer.Deserialize<AppConfig>(json)
                ?? throw new JsonException();
        }

        public static void SaveConfig(AppConfig config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CONFIG_PATH, json);
        }
    }
}
