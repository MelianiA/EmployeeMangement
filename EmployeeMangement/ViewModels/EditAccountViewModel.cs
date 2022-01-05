using EmployeeMangement.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMangement.ViewModels
{
    public class EditAccountViewModel : AppUser
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

    }
}
