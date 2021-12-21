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
            Employees.Add(new Employee { Id = 0, Name="Abdou", Departement="M'sila", Email="abd@meliani.eu"});
            Employees.Add(new Employee { Id = 1, Name="ab", Departement="Lyon", Email="abd@Lyon.eu"});
        }

        public Employee Get(int id) => Employees[id];

        public List<Employee> GetAll() => Employees;
    }
}
