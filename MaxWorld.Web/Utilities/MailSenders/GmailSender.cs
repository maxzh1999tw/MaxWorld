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

        public GmailSender(IConfiguration configuration)
        {
            _host = configuration["SMTP:Host"] ?? throw new KeyNotFoundException();
            _port = int.Parse(configuration["SMTP:Port"] ?? throw new KeyNotFoundException());
            _sendFrom = configuration["SMTP:SendFrom"] ?? throw new KeyNotFoundException();
            _sendName = configuration["SMTP:SendName"] ?? throw new KeyNotFoundException();
            _password = configuration["SMTP:Password"] ?? throw new KeyNotFoundException();
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
