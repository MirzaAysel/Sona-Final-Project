using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using Common.Helpers;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace SONA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;

        public BlogsController(IBlogService blogService, IWebHostEnvironment env)
        {
            _blogService = blogService;
            _env = env;
        }

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

            return View(blogs.ToPagedList(page,3));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }
            
            try
            {
                var fileName = await blog.ImageFile.CreateImage("blogs",_env.WebRootPath);
                blog.Image = fileName;
                blog.CreatedDate = DateTime.Now;
                blog.UpdatedDate = DateTime.Now;
                await _blogService.Create(blog);
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var path = _env.WebRootPath + "/uploads/blogs/";
            try
            {
                var data = await _blogService.Get(id);
                DeleteImage(path,data.Image);
                await _blogService.Delete(id);
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
        
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Blog blog;
         
            try
            {
                blog = await _blogService.Get(id);
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }
            
            var path = _env.WebRootPath + "/uploads/blogs/";
            try
            {
                if (blog.ImageFile != null)
                {
                    var fileName = await blog.ImageFile.CreateImage("blogs",_env.WebRootPath);
                    var data = await _blogService.Get(blog.Id);
                    DeleteImage(path,data.Image);
                    blog.Image = fileName;
                }
                else
                {
                    blog.Image = (await _blogService.Get(blog.Id)).Image;
                }

                await _blogService.Update(blog);
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
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Blog blog;
            try
            {
                blog = await _blogService.Get(id);
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
        
        private void DeleteImage(string dirName, string fileName)
        {
            System.IO.File.Delete(dirName + fileName);
        }
    }
}