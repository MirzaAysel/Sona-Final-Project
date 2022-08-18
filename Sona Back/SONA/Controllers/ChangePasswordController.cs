using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONA.ViewModels;

namespace SONA.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ChangePasswordController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
           
        }
        // GET
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ChangePasswordVM changePasswordVm)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user =>
                user.UserName == HttpContext.User.Identity.Name);

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,
                    changePasswordVm.CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(changePasswordVm);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            await _userManager.ResetPasswordAsync(user,token,changePasswordVm.Password);
            
            return RedirectToAction("Index","MyProfile");
        }
    }
}