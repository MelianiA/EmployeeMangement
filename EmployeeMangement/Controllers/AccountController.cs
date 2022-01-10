using EmployeeMangement.Models;
using EmployeeMangement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> ChekingExistingEmail(AccountRegisterViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Json(true);
            }
            else
                return Json($"This email {model.Email} is already used");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(AccountRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (User.IsInRole("Admin") && _signInManager.IsSignedIn(User))
                    {
                        return RedirectToAction("ListUsers", "Administration");

                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Employee");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AccountLoginViewModel model, string returnURL)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                                                                                model.Password, model.RememberMe,
                                                                                false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnURL))
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        return LocalRedirect(returnURL);
                    }
                }
                ModelState.AddModelError(string.Empty, "Login invalid attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Employee");
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    EditAccountViewModel model = new EditAccountViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        Id = id,
                        Password = user.PasswordHash,
                        ConfirmPassword = user.PasswordHash

                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Employee");
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Age = model.Age;

                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);

        }


        [HttpGet]
        public async Task<ActionResult> EditUser(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("NoteDound", $"user with id {id} cannot be found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            AccountEditUserViewModel model = new AccountEditUserViewModel
            {
                Age = user.Age,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Roles = userRoles,
                Claims = userClaims.Select(x => x.Value).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(AccountEditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return View("NoteDound", $"user with id {model.Id} cannot be found");
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Age = model.Age;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                await SetClaimsAndRoles(model);

            }
            else
            {
                await SetClaimsAndRoles(model);
            }
            return View(model);
        }

        public async Task<ActionResult> SetClaimsAndRoles(AccountEditUserViewModel model)
        {
            AppUser user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return View("NoteDound", $"user with id {model.Id} cannot be found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            model.Roles = userRoles;
            model.Claims = userClaims.Select(x => x.Value).ToList();

            return RedirectToAction("EditUser", model);

        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("NoteDound", $"user with id {id} cannot be found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return RedirectToAction("ListUsers", "Administration");

        }
    }
}
