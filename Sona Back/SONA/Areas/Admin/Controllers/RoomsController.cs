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
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IWebHostEnvironment _env;

        public RoomsController(IRoomService roomService,IWebHostEnvironment env)
        {
            _roomService = roomService;
            _env = env;
        }
        
        public async Task<IActionResult> Index(int page = 1)
        {

            List<Room> rooms;
            try
            {
                rooms = await _roomService.GetAll();
            }
            catch (Exception e)
            {
                return Json(new
                {
                    statusCode= 504,
                    message = e.Message
                });
            }
            return View(rooms.ToPagedList(page,3));
        }

        [HttpGet]
        public IActionResult Create()
        {
        
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (!ModelState.IsValid)
            {
                return View(room);
            }
            
            try
            {
                var fileName = await room.ImageFile.CreateImage("rooms",_env.WebRootPath);
                room.Image = fileName;
                await _roomService.Create(room);
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
            var path = _env.WebRootPath + "/uploads/rooms/";
            try
            {
                var data = await _roomService.Get(id);
                DeleteImage(path,data.Image);
                await _roomService.Delete(id);
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
            Room room;
         
            try
            {
                room = await _roomService.Get(id);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = 504,
                    message = e.Message
                });
            }
            return View(room);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Room room)
        {
            if (!ModelState.IsValid)
            {
                return View(room);
            }
            
            var path = _env.WebRootPath + "/uploads/rooms/";
            try
            {
                if (room.ImageFile != null)
                {
                    var fileName = await room.ImageFile.CreateImage("rooms",_env.WebRootPath);
                    var data = await _roomService.Get(room.Id);
                    DeleteImage(path,data.Image);
                    room.Image = fileName;
                }
                else
                {
                    room.Image = (await _roomService.Get(room.Id)).Image;
                }

                await _roomService.Update(room);
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
            Room room;
            try
            {
                room = await _roomService.Get(id);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = 504,
                    message = e.Message
                });
            }
            return View(room);
        }
        
        private void DeleteImage(string dirName,string fileName)
        {
            System.IO.File.Delete(dirName + fileName);
        }

    }
}