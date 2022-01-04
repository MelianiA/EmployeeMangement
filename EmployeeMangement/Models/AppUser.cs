﻿using Microsoft.AspNetCore.Identity;

namespace EmployeeMangement.Models
{
    public class AppUser: IdentityUser
    {
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
