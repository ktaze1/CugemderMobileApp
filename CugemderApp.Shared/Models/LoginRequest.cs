using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CugemderApp.Shared.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "E-mail alanı boş bırakılamaz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
