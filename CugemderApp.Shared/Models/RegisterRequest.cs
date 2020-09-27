using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CugemderApp.Shared.Models
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğunda; en az bir büyük, bir küçük ve bir adet özel karakter içermeli.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor!")]
        public string PasswordConfirm { get; set; }
    }
}
