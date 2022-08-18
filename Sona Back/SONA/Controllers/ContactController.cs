using System;
using System.Threading.Tasks;
using Business.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace SONA.Controllers
{
    public class ContactController : Controller
    {
        private readonly ICommentService _commentService;

        public ContactController(ICommentService commentService)
        {
            _commentService = commentService;
        }
     
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return View(comment);
            }
            
            comment.CreatedDate = DateTime.Now;
            comment.UpdatedDate = DateTime.Now;
            comment.IsContact = true;
            
            await _commentService.Create(comment);
            
            return RedirectToAction("Index");
        }
    }
}