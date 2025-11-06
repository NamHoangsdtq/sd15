using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.QLND
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
