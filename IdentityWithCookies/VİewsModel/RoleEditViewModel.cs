using IdentityWithCookies.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies.VİewsModel
{
    public class RoleList
    {
        public List<RoleEditViewModel> OwnerRole { get; set; }
        public List<RoleEditViewModel> AddRole { get; set; }
        public List<RoleEditViewModel> DeleteRole { get; set; }
        
        [Required(ErrorMessage ="User Id Gereklidir")]
        public string UserId { get; set; }

        public RoleList()
        {
            OwnerRole = new List<RoleEditViewModel>();
            AddRole = new List<RoleEditViewModel>();
            DeleteRole = new List<RoleEditViewModel>();
        }
    }
    public class RoleEditViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Exist { get; set; }
    }
}
