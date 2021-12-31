using EmployeeMangement.Models;
using EmployeeMangement.Models.Repositories;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ICompanyRepository<Employee> _companyRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmployeeController(ICompanyRepository<Employee> companyRepository,
            IWebHostEnvironment hostingEnvironment)
        {
            _companyRepository = companyRepository;
            this._hostingEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            return View(_companyRepository.GetEntities());
        }
        public ViewResult Details(int id)
        {
            return View(_companyRepository.Get(id));
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (ModelState.IsValid)
            {
                if (model.Photo != null)
                {
                    string extsion = Path.GetExtension(model.Photo.FileName);
                    if (!new List<string>() { ".jpg", ".png", ".jpeg", ".gif" }.Contains(extsion))
                    {
                        ModelState.AddModelError("", "Invalid file format");
                        return View(model);
                    }
                    uniqueFileName = CreateFilePhotoEmploye(model);
                }
                Employee employee = new Employee()
                {
                    Departement = model.Departement,
                    Email = model.Email,
                    Name = model.Name,
                    PhotoPath = uniqueFileName
                };
                _companyRepository.Add(employee);
                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View();
        }

        public ActionResult Delete(Employee employee)
        {
            if (ModelState.IsValid)
            {
                //delete img from server 
                Employee emp = _companyRepository.Get(employee.Id);
                if (emp.PhotoPath != null && emp.PhotoPath != "/Images/emp.png")
                {
                    var oldPhotoEmploye = Path.Combine(_hostingEnvironment.WebRootPath, "Images", emp.PhotoPath);
                    System.IO.File.Delete(oldPhotoEmploye);
                }
                //delete employee
                _companyRepository.Delete(employee);
                return RedirectToAction("Index", _companyRepository.GetEntities());
            }
            return View();
        }

        public ActionResult Update(int id)
        {

            Employee employee = _companyRepository.Get(id);
            EmployeeEditViewModel model = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Departement = employee.Departement,
                Email = employee.Email,
                PhotoPath = employee.PhotoPath
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(EmployeeEditViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string extsion = Path.GetExtension(model.Photo.FileName);
                if (!new List<string>() { ".jpg", ".png", ".jpeg", ".gif" }.Contains(extsion))
                {
                    ModelState.AddModelError("", "Invalid file format");
                    return View(model);
                }
                uniqueFileName = CreateFilePhotoEmploye(model);  
            }
            //delete img from server 
            Employee employee = _companyRepository.Get(model.Id);
            if (employee.PhotoPath != null && employee.PhotoPath != "/Images/emp.png")
            {
                var oldPhotoEmploye = Path.Combine(_hostingEnvironment.WebRootPath, "Images", employee.PhotoPath);
                System.IO.File.Delete(oldPhotoEmploye);
            }
            //update data
            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.Departement = model.Departement;
            employee.PhotoPath = uniqueFileName;

            //update db
            _companyRepository.Update(employee);
            return RedirectToAction("Index");
        }

        private string CreateFilePhotoEmploye(EmployeeCreateViewModel model)
        {
            string uniqueFileName;
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
            uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
            string path = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                model.Photo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
