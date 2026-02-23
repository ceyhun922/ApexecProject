using ApexWebAPI.Common;
using ApexWebAPI.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ApexWebAPI.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendMessageNotificationAsync(string fullName, string senderEmail, string phoneNumber, string messageBody)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fullName, _settings.SenderEmail));
            email.ReplyTo.Add(new MailboxAddress(fullName, senderEmail));
            email.To.Add(MailboxAddress.Parse(_settings.ReceiverEmail));
            email.Subject = $"Yeni Mesaj: {fullName} <{senderEmail}>";

            email.Body = new TextPart("html")
            {
                Text = $@"
                    <h3>Yeni İletişim Formu Mesajı</h3>
                    <p><b>Ad Soyad:</b> {fullName}</p>
                    <p><b>E-posta:</b> {senderEmail}</p>
                    <p><b>Telefon:</b> {phoneNumber}</p>
                    <hr/>
                    <p><b>Mesaj:</b></p>
                    <p>{messageBody}</p>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
