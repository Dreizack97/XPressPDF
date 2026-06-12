namespace BLL.Interfaces
{
    public interface IEmailService : IConnectionTestable
    {
        Task<bool> SendEmailAsync(string addresses, string subject, string body, List<string>? attachments = null);

        Task<bool> SendEmailAsync(List<string> addresses, string subject, string body, List<string>? attachments = null);
    }
}
