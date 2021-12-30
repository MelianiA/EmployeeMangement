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
            return View(_companyRepository.GetEntities());
        }
        public ViewResult Details(int id)
        {
            return View(_companyRepository.Get(id));
        }

        public ActionResult Create( )
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Photo = "Images/emp.png";
                _companyRepository.Add(employee);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _companyRepository.Delete(employee);
                return RedirectToAction("Index", new { id = employee.Id });
            }
            return View();
        }

        public ActionResult Update(int id)
        {
            _companyRepository.Get(id);
            return View(_companyRepository.Get(id));
        }

        [HttpPost]
        public ActionResult Update(Employee employee)
        {
                _companyRepository.Update(employee);
                return RedirectToAction("Index", new { id = employee.Id });
        }
    }
}
