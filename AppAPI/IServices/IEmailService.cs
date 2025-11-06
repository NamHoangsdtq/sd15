using AppData.ViewModels.QLND;

namespace AppAPI.IServices
{
    public interface IEmailService
    {
        Task SendForgotPasswordConfirmation(ForgotPasswordRequest forgot);
    }
}
