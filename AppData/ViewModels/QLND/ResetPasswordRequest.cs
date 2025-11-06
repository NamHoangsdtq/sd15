using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.QLND
{
    public class ResetPasswordRequest
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        public string ResetToken { get; set; }
        [Required]
        public string Password { get; set; }
        [Required,Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
