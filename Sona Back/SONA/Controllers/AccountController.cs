using System;
using System.Threading.Tasks;
using Common.Helpers;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SONA.ViewModels;
using X.PagedList;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SONA.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }

        // GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVm);
            }

            AppUser appUser = new AppUser()
            {
                FirstName = registerVm.FirstName,
                LastName = registerVm.LastName,
                Email = registerVm.Email,
                UserName = registerVm.FirstName.ToLower() + registerVm.LastName.ToLower(),
                CreatedDate = DateTime.Now,
                ImageUrl =
                    "https://upload.wikimedia.org/wikipedia/commons/thumb/5/59/User-avatar.svg/1200px-User-avatar.svg.png"
            };


            IdentityResult result = await _userManager.CreateAsync(appUser, registerVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }


            if ((await _context.Users.ToListAsync()).Count == 1)
            {
                await _userManager.AddToRoleAsync(appUser, Enums.Roles.Admin.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(appUser, Enums.Roles.Member.ToString());
            }
            
            return RedirectToAction("Login", "Account");
        }

        // GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            AppUser appUser = await _userManager.FindByEmailAsync(loginVm.Email);

            if (appUser is null)
            {
                ModelState.AddModelError("", "User not found!");
                return View();
            }

            SignInResult result =
                await _signInManager.PasswordSignInAsync(appUser, loginVm.Password, loginVm.RememberMe, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "User not found");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public async Task CreateRoles()
        {
            foreach (var role in Enum.GetNames(typeof(Enums.Roles)))
            {
                if (!(await _roleManager.RoleExistsAsync(role)))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}