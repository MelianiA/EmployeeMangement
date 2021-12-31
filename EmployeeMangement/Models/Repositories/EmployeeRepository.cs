using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models.Repositories
{
    public class EmployeeRepository : ICompanyRepository<Employee>
    {
        List<Employee> Employees;

        public EmployeeRepository()
        {
            Employees = new List<Employee>();
            Employees.Add(new Employee { Id = 0, 
                Name="Abdou", Departement="M'sila", Email="abd@meliani.eu"});
            Employees.Add(new Employee { Id = 1, 
                Name="ab", Departement="Lyon", Email="abd@Lyon.eu"});
        }

        public Employee Add(Employee employee)
        {
            employee.Id=Employees.Max(employee => employee.Id)+1;
            employee.PhotoPath = "/Images/emp.png";
            Employees.Add(employee);

            return employee;
        }

        public Employee Delete(Employee entityDeleted)
        {
            var employ =  Employees.Find(emp => emp.Id == entityDeleted.Id);
            if (employ != null) Employees.Remove(employ);
            return employ;
        }

        public Employee Get(int id) => Employees.SingleOrDefault(emp => emp.Id == id);

        public IEnumerable<Employee> GetEntities()
        {
            return Employees;
        }

        public Employee Update(Employee entityChanges)
        {
            var employ = Employees.Find(emp => emp.Id == entityChanges.Id);
            if (employ != null)
            {
                employ.Name = entityChanges.Name;
                employ.Departement = entityChanges.Departement;
                employ.Email = entityChanges.Email;
                employ.PhotoPath = entityChanges.PhotoPath;
            }
            return employ;
        }

    }
}
