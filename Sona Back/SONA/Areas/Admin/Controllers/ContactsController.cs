using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace SONA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactsController : Controller
    {
        private readonly ICommentService _commentService;

        public ContactsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = (await _commentService.GetAll()).Where(c => c.IsContact).ToList();
            return View(data.ToPagedList(page,10));
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Comment comment;
            try
            {
                comment = await _commentService.Get(id);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = 504,
                    message = e.Message
                });
            }
            return View(comment);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var data = await _commentService.Get(id);
                await _commentService.Delete(id);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = 504,
                    message = e.Message
                });
            }

            return RedirectToAction("Index");
        }
    }
}