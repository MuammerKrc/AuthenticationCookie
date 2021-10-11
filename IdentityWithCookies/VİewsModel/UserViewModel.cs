using IdentityWithCookies.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.VİewsModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Email adı gereklidir!!")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru formatta değil")]
        public string Email { get; set; }


        [Display(Name = "Telefon No")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "Şifre gereklidir!!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password, ErrorMessage = "Şifrenizi doğru formatta giriniz")]
        public string Password { get; set; }



        [Required(ErrorMessage = "Kullanıcı adı gereklidir!!")]
        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; }


        [Display(Name = "Şehir")]
        public string City { get; set; }



        [Display(Name = "Fotoğraf")]
        public string PictureUrl { get; set; }


        [Display(Name = "Doğum Tarihi ")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }


        public Gender Gender { get; set; }
    }
}
