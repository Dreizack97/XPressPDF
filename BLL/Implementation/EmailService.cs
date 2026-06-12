using BLL.Interfaces;
using BLL.Objects;
using MailKit.Net.Smtp;
using MimeKit;

namespace BLL.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfigManager _configManager;
        private readonly ILogManager _logManager;

        public EmailService(IConfigManager configManager, ILogManager logManager)
        {
            _configManager = configManager;
            _logManager = logManager;
        }

        private MailServerConfig MailServer => _configManager.Current.MailServer;

        public async Task<bool> ConnectAsync()
        {
            using SmtpClient smtpClient = new SmtpClient();

            try
            {
                await smtpClient.ConnectAsync(MailServer.Host, MailServer.Port, MailServer.SSL);
                return true;
            }
            catch (Exception ex)
            {
                await _logManager.LogAsync($"ERROR: Failed to connect to SMTP server - {ex.Message}", "ERROR");
                throw;
            }
            finally
            {
                if (smtpClient.IsConnected)
                    await smtpClient.DisconnectAsync(true);
            }
        }

        public async Task<bool> SendEmailAsync(string addresses, string subject, string body, List<string>? attachments = null)
            => await SendEmailAsync(new List<string> { addresses }, subject, body, attachments);

        public async Task<bool> SendEmailAsync(List<string> addresses, string subject, string body, List<string>? attachments = null)
        {
            MailServerConfig mailServer = MailServer;

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(mailServer.DisplayName, mailServer.Address));

            foreach (string address in addresses)
                message.To.Add(MailboxAddress.Parse(address));

            message.Subject = subject;

            BodyBuilder builder = new BodyBuilder { HtmlBody = body };

            if (attachments != null)
                foreach (string attachment in attachments)
                    if (!string.IsNullOrWhiteSpace(attachment))
                        builder.Attachments.Add(attachment);

            message.Body = builder.ToMessageBody();

            using SmtpClient smtpClient = new SmtpClient();

            try
            {
                await smtpClient.ConnectAsync(mailServer.Host, mailServer.Port, mailServer.SSL);
                await smtpClient.AuthenticateAsync(mailServer.Address, mailServer.Password);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                await _logManager.LogAsync($"ERROR: Failed to send email - {ex.Message}", "ERROR");
                throw;
            }
        }
    }
}
