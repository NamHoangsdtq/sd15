using AppAPI.IServices;
using AppData.ViewModels.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailServices _iMailServices;
        public MailController(IMailServices mailServices)
        {
            _iMailServices = mailServices;
        }
        [HttpPost("SendMail")]
        public bool SendMail(MailData mailData)
        {
            return _iMailServices.SendMail(mailData).Result;
        }
    }
}
