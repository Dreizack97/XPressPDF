using BLL.Interfaces;
using BLL.Objects;
using System.Text.Json;

namespace BLL.Utilities
{
    public class ConfigManager : IConfigManager
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { WriteIndented = true };

        private readonly string _configPath;
        private readonly object _sync = new object();
        private AppConfig? _current;

        public ConfigManager()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"))
        {
        }

        public ConfigManager(string configPath)
        {
            _configPath = configPath;
        }

        public AppConfig Current
        {
            get
            {
                lock (_sync)
                {
                    return _current ??= LoadOrCreate();
                }
            }
        }

        public void Save(AppConfig config)
        {
            lock (_sync)
            {
                WriteConfig(config);
                _current = config;
            }
        }

        public void Reload()
        {
            lock (_sync)
            {
                _current = null;
            }
        }

        private AppConfig LoadOrCreate()
        {
            if (File.Exists(_configPath))
            {
                string json = File.ReadAllText(_configPath);

                return JsonSerializer.Deserialize<AppConfig>(json)
                    ?? throw new JsonException($"Invalid configuration file: {_configPath}");
            }

            AppConfig defaultConfig = CreateDefaultConfig();
            WriteConfig(defaultConfig);

            return defaultConfig;
        }

        private void WriteConfig(AppConfig config)
        {
            string json = JsonSerializer.Serialize(config, SerializerOptions);
            File.WriteAllText(_configPath, json);
        }

        private static AppConfig CreateDefaultConfig() => new AppConfig
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
    }
}
