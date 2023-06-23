namespace MaxWorld.Web.Utilities.MailSenders
{
    public interface IMailSender
    {
        Task SendMailAsync(MailData mailData);
    }
}
