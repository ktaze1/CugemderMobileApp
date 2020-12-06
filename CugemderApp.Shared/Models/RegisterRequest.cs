using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CugemderApp.Shared.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [RegularExpression(@"^\(?([0-9]{4})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateofBirth { get; set; }


        [EmailAddress]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Email { get; set; }

        public string photoUrl { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğunda; en az bir büyük, bir küçük ve bir adet özel karakter içermeli.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor!")]
        public string PasswordConfirm { get; set; }
    }
}
