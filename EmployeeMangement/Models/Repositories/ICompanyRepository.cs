using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models.Repositories
{
    public interface ICompanyRepository<TEntity>
    {
        TEntity Get(int id);
    }
}
