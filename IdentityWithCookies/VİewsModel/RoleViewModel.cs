using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.VİewsModel
{
    public class RoleViewModel
    {
        [Required(ErrorMessage ="Role ismi gereklidir")]
        [Display(Name ="Role Adı")]
        public string Name { get; set; }


        public string Id { get; set; }
    }
}
