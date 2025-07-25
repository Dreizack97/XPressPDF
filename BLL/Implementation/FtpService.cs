using BLL.Interfaces;
using BLL.Objetcs;
using FluentFTP;
using FluentFTP.Exceptions;
using System.Threading.Tasks;

namespace BLL.Utilities
{
    public class FtpService : IFtpService
    {
        private readonly FtpClient ftpClient;

        private struct FtpServer
        {
            public const string HOST = "";
            public const string USER = "";
            public const string PASSWORD = "";
            public const int PORT = 21;
            public const string ROOTPATH = "";
        }

        public FtpService()
        {
            ftpClient = new FtpClient(FtpServer.HOST, FtpServer.USER, FtpServer.PASSWORD, FtpServer.PORT);
        }

        public async Task<List<FileUploadResult>> UploadAsync(string filePath, string uploadPath)
            => await UploadAsync([filePath], uploadPath);

        public async Task<List<FileUploadResult>> UploadAsync(string[] filesPath, string uploadPath)
        {
            ArgumentNullException.ThrowIfNull(filesPath);
            ArgumentNullException.ThrowIfNull(uploadPath);

            try
            {
                ftpClient.Connect();
                await LogManager.LogAsync($"Successfully connected to FTP server.", "INFO");

                List<FileUploadResult> results = new List<FileUploadResult>();

                string remoteBaseDirectory = Path.Combine(FtpServer.ROOTPATH, uploadPath).Replace(@"\", "/");
                await LogManager.LogAsync($"Starting multiple file upload to: {remoteBaseDirectory}", "INFO");

                foreach (string filePath in filesPath)
                {
                    string fileName = Path.GetFileName(filePath);
                    string remotePath = Path.Combine(remoteBaseDirectory, fileName);

                    FtpStatus status = ftpClient.UploadFile(filePath, remotePath, FtpRemoteExists.Overwrite, true);

                    if (status == FtpStatus.Success)
                    {
                        results.Add(new FileUploadResult(filePath, remotePath, true));
                        await LogManager.LogAsync($"SUCCESS: '{filePath}' uploaded.", "INFO");
                    }
                    else
                    {
                        results.Add(new FileUploadResult(filePath, remotePath, false, $"FTP upload failed. Status: {status}"));
                        await LogManager.LogAsync($"FAILED: '{filePath}' - Status: {status}", "ERROR");
                    }
                }

                return results;
            }
            catch (FtpException ex)
            {
                await LogManager.LogAsync($"FATAL ERROR: Failed to connect to FTP server - {ex.Message}", "CRITICAL");
                throw;
            }
            catch (Exception ex)
            {
                await LogManager.LogAsync($"FATAL ERROR: An unexpected error occurred during multiple file upload - {ex.Message}", "CRITICAL");
                throw;
            }
            finally
            {
                if (ftpClient.IsConnected)
                {
                    ftpClient.Disconnect();
                    await LogManager.LogAsync("Disconnected from FTP server.", "INFO");
                }

                await LogManager.LogAsync("Finished multiple file upload operation.", "INFO");
            }
        }
    }
}
