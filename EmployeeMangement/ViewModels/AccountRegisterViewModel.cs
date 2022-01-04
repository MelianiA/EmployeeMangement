using EmployeeMangement.Tools;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMangement.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote("ChekingExistingEmail", "Account")]
        [ValidEmailDomainAttribute("iasoft.fr;gmail.com;hotmail.com;hotmail.fr",
            ErrorMessage ="Email domain mmust be in isafot.fr or gmail.com or hotmail.fr or hotmail.com")]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(10,70)]
        public int Age { get; set; }
    }
}
