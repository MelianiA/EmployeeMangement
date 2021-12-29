using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeMangement.Models.Repositories
{
    public class SQLEmployeeRepository : ICompanyRepository<Employee>
    {
        private readonly AppDbContext _context;

        public SQLEmployeeRepository(AppDbContext appDbContext)
        {
            this._context = appDbContext;
        }

        public Employee Add(Employee entity)
        {
            this._context.Employees.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Employee Delete(Employee entityDeleted)
        {
            var emp = Get(entityDeleted.Id);
            if (true)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();

            }
            return emp;
        }

        public Employee Get(int id)
        {
            return _context.Employees.SingleOrDefault(emp => emp.Id == id);
        }

        public IEnumerable<Employee> GetEntities()
        {
            return _context.Employees;
        }

        public Employee Update(Employee entityChanges)
        {
            var emp = _context.Employees.Attach(entityChanges);
            emp.State =  EntityState.Modified;
            _context.SaveChanges();
            return entityChanges;
        }
    }
}
