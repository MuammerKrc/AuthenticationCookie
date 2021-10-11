using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.VİewsModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email alanı gereklidir.")]
        [DataType(dataType: DataType.EmailAddress,ErrorMessage ="Email alanını doğru formatta giriniz.")]
        [Display(Name ="Email Adres")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(dataType:DataType.Password)]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }

    }
}
