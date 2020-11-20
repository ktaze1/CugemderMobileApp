using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CugemderApp.Shared.Models
{
    public class ForgotPasswordModel
    {

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string email { get; set; }
    }
}
