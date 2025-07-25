using BLL.Interfaces;
using BLL.Objetcs;
using FluentFTP;
using FluentFTP.Exceptions;

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

        public List<FileUploadResult> Upload(string filePath, string uploadPath)
            => Upload([filePath], uploadPath);

        public List<FileUploadResult> Upload(string[] filesPath, string uploadPath)
        {
            ArgumentNullException.ThrowIfNull(filesPath);
            ArgumentNullException.ThrowIfNull(uploadPath);

            try
            {
                ftpClient.Connect();
                List<FileUploadResult> results = new List<FileUploadResult>();

                foreach (string filePath in filesPath)
                {
                    string fileName = Path.GetFileName(filePath);
                    string remotePath = Path.Combine(FtpServer.ROOTPATH, uploadPath, fileName);

                    FtpStatus status = ftpClient.UploadFile(filePath, remotePath, FtpRemoteExists.Overwrite, true);

                    if (status == FtpStatus.Success)
                        results.Add(new FileUploadResult(filePath, remotePath, true));
                    else
                        results.Add(new FileUploadResult(filePath, remotePath, false, $"FTP upload failed. Status: {status}"));
                }

                return results;
            }
            catch (FtpException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (ftpClient.IsConnected)
                    ftpClient.Disconnect();
            }
        }
    }
}
