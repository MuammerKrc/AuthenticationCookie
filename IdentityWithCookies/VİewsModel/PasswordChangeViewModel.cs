using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.VİewsModel
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage ="Bu alan gereklidir")]
        [Display(Name ="Eski şifreniz")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 krakter girmelisiniz")]
        public string PasswordOld { get; set; }



        [Required(ErrorMessage = "Bu alan gereklidir")]
        [Display(Name ="Yeni şifreniz")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="En az 4 krakter girmelisiniz")]
        public string PasswordNew { get; set; }



        [Display(Name ="Yeni şifre tekrarı")]
        [Required(ErrorMessage = "Bu alan gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "En az 4 krakter girmelisiniz")]
        [Compare("PasswordNew",ErrorMessage ="Eşleşme yapılamadı")]
        public string PasswordConfirm { get; set; }
    }
}
