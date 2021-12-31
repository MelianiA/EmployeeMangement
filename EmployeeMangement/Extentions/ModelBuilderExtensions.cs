using EmployeeMangement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMangement.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                   new Employee()
                   {
                       Id = 44,
                       Name = "Abderrahmen",
                       Departement = "Networking",
                       Email = "abd@meliani.eu",
                       PhotoPath = "/Images/emp.png"
                   }
               );
        }
    }
}
