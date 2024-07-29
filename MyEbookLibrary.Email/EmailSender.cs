using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _configuration;
        public EmailSender(EmailConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(EmailMessage email)
        {
            var message = CreateEmailMessage(email);
            await SendAsync(message);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress("Ebook Library", _configuration.From));
            email.Subject = message.Subject;

            if (message.IsHtml)
            {
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = message.Body;
                email.Body = builder.ToMessageBody();
            }
            else
            {
                email.Body = new TextPart("plain") { Text = message.Body };
            }
            
            email.To.AddRange(message.To);
            return email;

        }

        private async Task SendAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_configuration.SmtpServer, _configuration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_configuration.UserName, _configuration.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }

            }
        }
    }
}
