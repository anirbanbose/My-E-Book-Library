using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Email
{
    public interface IEmailSender
    {
        Task SendEmail(EmailMessage email);
    }
}
