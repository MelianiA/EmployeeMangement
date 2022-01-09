using EmployeeMangement.Tools;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMangement.ViewModels
{
    public class AccountEditUserViewModel
    {
        public AccountEditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [ValidEmailDomainAttribute("iasoft.fr;gmail.com;hotmail.com;hotmail.fr",
            ErrorMessage = "Email domain mmust be in isafot.fr or gmail.com or hotmail.fr or hotmail.com")]
        public string Email { get; set; }

        public int Age { get; set; }

        public IList<string> Roles { get; set; }
        public IList<string> Claims { get; set; }
    }
}
