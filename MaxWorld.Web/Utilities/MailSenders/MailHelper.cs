namespace MaxWorld.Web.Utilities.MailSenders
{
    public class MailHelper
    {
        private IMailSender _mailSender;

        public MailHelper(IMailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public Task SendAsync(string to, string subject, string body)
        {
            var mail = new MailData(new string[] { to }, subject, body);
            return _mailSender.SendMailAsync(mail);
        }
    }
}
