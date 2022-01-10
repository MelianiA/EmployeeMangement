using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMangement.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string  Id { get; set;}
        [Required]
        [Display(Name = "Role name")]
        public string RoleName { get; set;}
        public List<string> Users { get; set; }
    }
}
