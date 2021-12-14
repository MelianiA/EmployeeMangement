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
            Employees.Add(new Employee { Name="Abdou"});
        }

        public Employee Get(int id)
        {
            return Employees[id];
        }
    }
}
