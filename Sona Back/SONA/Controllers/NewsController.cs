using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace SONA.Controllers
{
    public class NewsController : Controller
    {
        private readonly IBlogService _blogService;

        public NewsController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        // GET
        public async Task<IActionResult> Index(int page = 1)
        {
            List<Blog> blogs;
            try
            {
                blogs = await _blogService.GetAll();
            }
            catch (Exception e)
            {
                return Json(new
                {
                    statusCode = 504,
                    message = e.Message
                });
            }

            return View(blogs.ToPagedList(page,6));
        }
        
        public async Task<IActionResult> Details(int id)
        {
            Blog blog;
            try
            {
                blog = await _blogService.Get(id);
                ViewBag.Data = (await _blogService.GetAll()).OrderBy(b => b.CreatedDate).Take(3).ToList();
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = 504,
                    message = e.Message
                });
            }
            return View(blog);
        }
    }
}