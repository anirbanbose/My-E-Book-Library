using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Email.Emails
{
    
    public class AccountEmails : IAccountEmails
    {
        private readonly IEmailSender _emailSender;
        public AccountEmails(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendRegistrationEmail(string email)
        {
            var message = new EmailMessage([email], "New account created", "Your account has been created. Please <a href=\"http://localhost:4200/account/login\">login</a> here.", true);
            await _emailSender.SendEmail(message);
        }

        public async Task SendProfileUpdateEmail(string email)
        {
            var message = new EmailMessage([email], "Profile updated", "Your profile has been updated. Please <a href=\"http://localhost:4200/account/login\">login</a> here.", true);
            await _emailSender.SendEmail(message);
        }
    }

    public interface IAccountEmails
    {
        Task SendRegistrationEmail(string email);
        Task SendProfileUpdateEmail(string email);
    }
}
