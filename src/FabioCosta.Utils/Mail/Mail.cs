using FabioCosta.Utils.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace FabioCosta.Utils.Mail
{
    public static class Mail
    {
        #region Public

        public static async Task SendEmailAsync(NotificationMetadata notificationMetadata, ContactEmail contactEmail)
        {
            EmailMessage message = CreateEmailMessageFromContactEmail(notificationMetadata, contactEmail);
            MimeMessage mimeMessage = CreateMimeMessageFromEmailMessage(message);

            using SmtpClient smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(notificationMetadata.SmtpServer, notificationMetadata.Port, true);
            await smtpClient.AuthenticateAsync(notificationMetadata.UserName, notificationMetadata.Password);
            await smtpClient.SendAsync(mimeMessage);
            await smtpClient.DisconnectAsync(true);
        }

        #endregion

        #region Private

        private static EmailMessage CreateEmailMessageFromContactEmail(NotificationMetadata notificationMetadata, ContactEmail contactEmail)
        {
            var message = new EmailMessage
            {
                Sender = new MailboxAddress("Self", notificationMetadata.Sender),
                Reciever = new MailboxAddress("Self", notificationMetadata.Reciever),
                Subject = contactEmail.Subject,
                Content = $"From: {contactEmail.Name} - {contactEmail.Email}\nMessage: {contactEmail.Message}"
            };

            return message;
        }

        private static MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            { 
                Text = message.Content 
            };

            return mimeMessage;
        }

        #endregion
    }
}
