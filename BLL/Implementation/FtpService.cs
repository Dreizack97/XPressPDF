using BLL.Interfaces;
using BLL.Objects;
using FluentFTP;
using FluentFTP.Exceptions;

namespace BLL.Implementation
{
    public class FtpService : IFtpService
    {
        private readonly IConfigManager _configManager;
        private readonly ILogManager _logManager;

        public FtpService(IConfigManager configManager, ILogManager logManager)
        {
            _configManager = configManager;
            _logManager = logManager;
        }

        public async Task<bool> ConnectAsync()
        {
            await using AsyncFtpClient ftpClient = CreateClient();

            try
            {
                await ftpClient.Connect();
                return ftpClient.IsConnected;
            }
            catch (FtpException ex)
            {
                await _logManager.LogAsync($"FATAL ERROR: Failed to connect to FTP server - {ex.Message}", "CRITICAL");
                throw;
            }
            catch (Exception ex)
            {
                await _logManager.LogAsync($"FATAL ERROR: An unexpected error occurred while connecting to FTP server - {ex.Message}", "CRITICAL");
                throw;
            }
            finally
            {
                if (ftpClient.IsConnected)
                {
                    await ftpClient.Disconnect();
                    await _logManager.LogAsync("Disconnected from FTP server.", "INFO");
                }
            }
        }

        public async Task<List<FileUploadResult>> UploadAsync(string filePath, string uploadPath)
            => await UploadAsync([filePath], uploadPath);

        public async Task<List<FileUploadResult>> UploadAsync(string[] filesPath, string uploadPath)
        {
            ArgumentNullException.ThrowIfNull(filesPath);
            ArgumentNullException.ThrowIfNull(uploadPath);

            await using AsyncFtpClient ftpClient = CreateClient();

            try
            {
                await ftpClient.Connect();
                await _logManager.LogAsync("Successfully connected to FTP server.", "INFO");

                List<FileUploadResult> results = new List<FileUploadResult>();

                string remoteBaseDirectory = Path.Combine(_configManager.Current.FtpServer.RootPath, uploadPath).Replace(@"\", "/");
                await _logManager.LogAsync($"Starting multiple file upload to: {remoteBaseDirectory}", "INFO");

                foreach (string filePath in filesPath)
                {
                    string fileName = Path.GetFileName(filePath);
                    string remotePath = Path.Combine(remoteBaseDirectory, fileName).Replace(@"\", "/");

                    FtpStatus status = await ftpClient.UploadFile(filePath, remotePath, FtpRemoteExists.Overwrite, true);

                    if (status == FtpStatus.Success)
                    {
                        results.Add(new FileUploadResult(filePath, remotePath, true));
                        await _logManager.LogAsync($"SUCCESS: '{filePath}' uploaded.", "INFO");
                    }
                    else
                    {
                        results.Add(new FileUploadResult(filePath, remotePath, false, $"FTP upload failed. Status: {status}"));
                        await _logManager.LogAsync($"FAILED: '{filePath}' - Status: {status}", "ERROR");
                    }
                }

                return results;
            }
            catch (FtpException ex)
            {
                await _logManager.LogAsync($"FATAL ERROR: Failed to connect to FTP server - {ex.Message}", "CRITICAL");
                throw;
            }
            catch (Exception ex)
            {
                await _logManager.LogAsync($"FATAL ERROR: An unexpected error occurred during multiple file upload - {ex.Message}", "CRITICAL");
                throw;
            }
            finally
            {
                if (ftpClient.IsConnected)
                {
                    await ftpClient.Disconnect();
                    await _logManager.LogAsync("Disconnected from FTP server.", "INFO");
                }

                await _logManager.LogAsync("Finished multiple file upload operation.", "INFO");
            }
        }

        // El cliente se crea por operación para usar siempre la configuración vigente,
        // que puede cambiar desde la pantalla de configuración durante la sesión.
        private AsyncFtpClient CreateClient()
        {
            FtpServerConfig config = _configManager.Current.FtpServer;

            return new AsyncFtpClient(config.Host, config.User, config.Password, config.Port);
        }
    }
}
