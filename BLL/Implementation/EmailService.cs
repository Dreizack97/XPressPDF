using BLL.Interfaces;
using BLL.Objects;
using BLL.Utilities;
using MailKit.Net.Smtp;
using MimeKit;

namespace BLL.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailServerConfig _mailServerConfig;
        private readonly AppConfig _config;

        public EmailService()
        {
            _config = ConfigManager.LoadConfig();

            _mailServerConfig = _config.MailServer;
        }

        public async Task<bool> ConnectAsync()
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                try
                {
                    await smtpClient.ConnectAsync(_mailServerConfig.Host, _mailServerConfig.Port, _mailServerConfig.SSL);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await smtpClient.DisconnectAsync(true);
                }
            }
        }

        public async Task<bool> SendEmailAsync(string addresses, string subject, string body, List<string>? attachments = null)
            => await SendEmailAsync(new List<string> { addresses }, subject, body, attachments);

        public async Task<bool> SendEmailAsync(List<string> addresses, string subject, string body, List<string>? attachments = null)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailServerConfig.DisplayName, _mailServerConfig.Address));

            foreach (string address in addresses)
                message.To.Add(MailboxAddress.Parse(address));

            message.Subject = subject;

            BodyBuilder builder = new BodyBuilder { HtmlBody = body };

            if (attachments != null)
                foreach (string attachment in attachments)
                    if (!string.IsNullOrWhiteSpace(attachment))
                        builder.Attachments.Add(attachment);

            message.Body = builder.ToMessageBody();

            using (SmtpClient smtpClient = new SmtpClient())
            {
                try
                {
                    await smtpClient.ConnectAsync(_mailServerConfig.Host, _mailServerConfig.Port, _mailServerConfig.SSL);
                    await smtpClient.AuthenticateAsync(_mailServerConfig.Address, _mailServerConfig.Password);
                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
