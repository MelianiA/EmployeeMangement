using EmployeeMangement.Models;
using EmployeeMangement.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Controllers
{
    public class EmployeeController : Controller
    {
        private ICompanyRepository<Employee> _companyRepository;
        public EmployeeController(ICompanyRepository<Employee> companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public ViewResult Index()
        {
            return View(_companyRepository.GetAll());
        }
        public ViewResult Details(int id)
        {
            return View(_companyRepository.Get(id));
        }
    }
}
