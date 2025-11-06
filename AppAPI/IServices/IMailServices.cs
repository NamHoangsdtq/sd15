using AppData.ViewModels.Mail;

namespace AppAPI.IServices
{
    public interface IMailServices
    {
        Task<bool> SendMail(MailData mailData);
    }
}
