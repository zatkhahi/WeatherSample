using Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Common.Presentation.Services
{
    public class MailKitService : IMailService
    {

        private readonly MailSettings _mailSettings;
        private readonly IBackgroundTaskQueue queue;

        public MailKitService(IOptions<MailSettings> mailSettings, IBackgroundTaskQueue queue)
        {
            _mailSettings = mailSettings.Value;
            this.queue = queue;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            queue.QueueBackgroundWorkItem(async token =>
            {
                await SendEmail(mailRequest);
            });
        }

        public async Task SendEmail(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Content?.Length > 0)
                    {
                        builder.Attachments.Add(file.Name, file.Content, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
