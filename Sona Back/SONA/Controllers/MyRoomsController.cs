using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SONA.Controllers
{
    [Authorize]
    public class MyRoomsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        // GET
        public MyRoomsController(UserManager<AppUser> userManager,AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _context.Users.Where(us => us.UserName == HttpContext.User.Identity.Name).Include(user => user.Reservations).ThenInclude(r => r.Room).FirstOrDefaultAsync();
            return View(data);
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            _context.Reservations.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Cancel(int id)
        {
            var data = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            data.IsCancelled = true;
            _context.Reservations.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}