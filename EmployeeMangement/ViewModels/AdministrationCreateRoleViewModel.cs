using System.ComponentModel.DataAnnotations;

namespace EmployeeMangement.ViewModels
{
    public class AdministrationCreateRoleViewModel
    {
        [Required]
        [Display(Name ="Role name")]
        public string RoleName { get; set; }
    }
}
