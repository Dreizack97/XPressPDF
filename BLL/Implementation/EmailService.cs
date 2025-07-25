using BLL.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace BLL.Implementation
{
    public class EmailService : IEmailService
    {
        private struct MailServer
        {
            public const string ADDRESS = "";

            public const string PASSWORD = "";

            public const string DISPLAY_NAME = "";

            public const string HOST = "";

            public const int PORT = 587;

            public const bool SSL = true;
        }

        public async Task<bool> SendEmailAsync(string addresses, string subject, string body, List<string>? attachments = null)
            => await SendEmailAsync(new List<string> { addresses }, subject, body, attachments);

        public async Task<bool> SendEmailAsync(List<string> addresses, string subject, string body, List<string>? attachments = null)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(MailServer.DISPLAY_NAME, MailServer.ADDRESS));

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
                    await smtpClient.ConnectAsync(MailServer.HOST, MailServer.PORT, MailServer.SSL);
                    await smtpClient.AuthenticateAsync(MailServer.ADDRESS, MailServer.PASSWORD);
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
