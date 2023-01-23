using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace VAT_TODOLIST.Models
{
    public class EmailMessageContent
    {

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailMessageContent(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("HMS", x)));
            Subject = subject;
            Content = content;
        }
    }
}
