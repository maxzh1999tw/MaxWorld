using System.Net.Mail;

namespace MaxWorld.Web.Utilities.MailSenders
{
    public class GmailSender : IMailSender
    {
        private string _host;
        private int _port;
        private string _sendName;
        private string _sendFrom;
        private string _password;

        public GmailSender(string host, int port, string sendFrom, string sendName, string password)
        {
            _host = host;
            _port = port;
            _sendFrom = sendFrom;
            _sendName = sendName;
            _password = password;
        }

        public async Task SendMailAsync(MailData mailData)
        {
            using var mail = new MailMessage()
            {
                Subject = mailData.Subject,
                Body = mailData.Body,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
                From = new MailAddress(_sendFrom, _sendName),
            };

            foreach (var email in mailData.To)
            {
                mail.To.Add(email);
            }

            if (mailData.CC != null)
            {
                foreach (var email in mailData.CC)
                {
                    mail.CC.Add(email);
                }
            }

            if (mailData.Attachments != null)
            {
                foreach (var attachment in mailData.Attachments)
                {
                    mail.Attachments.Add(attachment);
                }
            }

            var smtp = new SmtpClient(_host, _port)
            {
                Credentials = new System.Net.NetworkCredential(_sendFrom, _password),
                EnableSsl = true,
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
