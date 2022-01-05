using EmployeeMangement.Models;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateRole(AdministrationCreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

        public ActionResult ListRoles() => View(_roleManager.Roles);

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id is null)
            {
                return View("NotFound", "Please add role Id in the URL");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("NotFound", $"Cannot find role with Id: {id}");
            }
            EditRoleViewModel model = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()

            };

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.Email);
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<ActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role is null)
                {
                    return View("NotFound", $"The role id {model.Id} cannot be found !");
                }
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditUserRole(string idRole)
        {
            if (string.IsNullOrEmpty(idRole))
            {
                return View("NotFound", $"The role must exist and not empty in the URL !");
            }
            var role = await _roleManager.FindByIdAsync(idRole);
            if (role is null)
            {
                return View("NotFound", $"The role id {idRole} cannot be found !");
            }
            List<EditUsersRoleViewModel> Models = new List<EditUsersRoleViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var model = new EditUsersRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = false
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.IsSelected = true;
                }

                Models.Add(model);

            }
            ViewBag.RoleId = idRole;
            return View(Models);
        }

        [HttpPost]
        public async Task<ActionResult> EditUserRole(string idRole, List<EditUsersRoleViewModel> models)
        {
            if (string.IsNullOrEmpty(idRole))
            {
                return View("NotFound", $"The role must exist and not empty in the URL !");
            }
            var role = await _roleManager.FindByIdAsync(idRole);
            if (role is null)
            {
                return View("NotFound", $"The role id {idRole} cannot be found !");
            }
            IdentityResult result = null;
            for (int i = 0; i < models.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(models[i].UserId);
                if (await _userManager.IsInRoleAsync(user, role.Name) && !models[i].IsSelected)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                if (!(await _userManager.IsInRoleAsync(user, role.Name)) && models[i].IsSelected)
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
            if (result != null &&  !result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return RedirectToAction(nameof(Edit), new { id = idRole });

        }
    }
}
