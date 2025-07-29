namespace BLL.Interfaces
{
    public interface IEmailService
    {
        Task<bool> ConnectAsync();

        Task<bool> SendEmailAsync(string addresses, string subject, string body, List<string>? attachments = null);

        Task<bool> SendEmailAsync(List<string> addresses, string subject, string body, List<string>? attachments = null);
    }
}
