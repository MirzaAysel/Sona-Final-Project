using System.Threading.Tasks;
using Common.Helpers;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONA.ViewModels;

namespace SONA.Controllers
{
    [Authorize]
    public class EditProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;


        public EditProfileController(UserManager<AppUser> userManager,AppDbContext context,IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }
        
        // GET
        public async Task<IActionResult>  Index()
        {
            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            UserVM userVm = new UserVM
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Image = currentUser.ImageUrl,
                Email = currentUser.Email
            };
            return View(userVm);
        }
        
        [HttpPost]
        public async Task<IActionResult>  Index(UserVM userVm)
        {
            var User = await _context.Users.FirstOrDefaultAsync(user =>
                user.UserName == HttpContext.User.Identity.Name);
            var path = _environment.WebRootPath + "/uploads/users/";

       

            User.FirstName = userVm.FirstName;
            User.LastName = userVm.LastName;
            userVm.Email = userVm.Email;

            if (userVm.ImageFile != null)
            {
                var fileName = await userVm.ImageFile.CreateImage("users",_environment.WebRootPath);
                if (System.IO.File.Exists(path + User.ImageUrl))
                {
                    DeleteImage(path,User.ImageUrl);
                }
                User.ImageUrl = fileName;
            }

            _context.Users.Update(User);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","MyProfile");
        }
        
        private void DeleteImage(string dirName,string fileName)
        {
            System.IO.File.Delete(dirName + fileName);
        }
    }
}