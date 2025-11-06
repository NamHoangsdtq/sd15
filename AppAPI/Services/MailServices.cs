using AppAPI.IServices;
using AppData.ViewModels.Mail;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace AppAPI.Services
{
    public class MailServices : IMailServices
    {
        private readonly MailSettings _mailSettings;
        public MailServices(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }
        public async Task<bool> SendMail(MailData mailData)
        {
            try
            {
                using(MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));
                    emailMessage.Subject = mailData.EmailSubject;
                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    using(SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
