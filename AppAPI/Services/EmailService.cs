using System.Net.Mail;
using System.Net;
using AppData.ViewModels.QLND;
using AppAPI.IServices;

namespace AppAPI.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendForgotPasswordConfirmation(ForgotPasswordRequest forgot)
        {
            // Create an instance of SmtpClient
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("kenishop22@gmail.com", "Nvloi7503");

            // Create an instance of MailMessage
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("kenishop22@gmail.com");
            mailMessage.To.Add(new MailAddress(forgot.Email));
            mailMessage.Subject = "Reset Password";
            mailMessage.Body = "Please click the link below to reset your password:<br><a href='https://localhost:7095/api/ResetPassword?email=" + forgot.Email + "&token=" + forgot.Token + "'>Reset Password</a>";

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);
            mailMessage.Dispose();
        }
    }
}
