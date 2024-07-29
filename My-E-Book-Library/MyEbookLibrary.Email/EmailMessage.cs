using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Email
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string body, bool isHtml = false)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(d => new MailboxAddress("To", d)));

            Subject = subject;
            Body = body;
            IsHtml = isHtml;
        }
    }
}
