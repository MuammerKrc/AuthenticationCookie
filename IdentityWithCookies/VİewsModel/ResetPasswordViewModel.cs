using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.VİewsModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Geçerli bir email giriniz.")]
        [System.ComponentModel.DataAnnotations.Display(Name ="Email Adres")]
        public string Email { get; set; }
    }
}
