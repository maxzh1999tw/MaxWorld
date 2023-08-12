using System.Net.Mail;

namespace MaxWorld.Web.Utilities.MailSenders
{
    public class MailData
    {
        public IEnumerable<string> To { get; set; }
        public IEnumerable<string>? CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<Attachment>? Attachments { get; set; }

        public MailData(IEnumerable<string> to, string subject, string body, IEnumerable<string>? cc = null, IEnumerable<Attachment>? attachments = null)
        {
            To = to;
            CC = cc;
            Subject = subject;
            Body = body;
            Attachments = attachments;
        }
    }
}
