using Microsoft.EntityFrameworkCore;

namespace EmployeeMangement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                    new Employee() { Id = 1, Name = "abdou", Departement = "Networking", Email = "abd@meliani.eu", Photo = "/Images/emp.png" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
