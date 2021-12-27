﻿using Microsoft.EntityFrameworkCore;

namespace EmployeeMangement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
