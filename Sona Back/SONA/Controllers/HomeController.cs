using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONA.ViewModels;

namespace SONA.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRoomService _roomService;
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;

        public HomeController(AppDbContext context,IRoomService roomService,IBlogService blogService,ICommentService commentService)
        {
            _context = context;
            _roomService = roomService;
            _blogService = blogService;
            _commentService = commentService;
        }
      
        public async Task<IActionResult> Index()
        {
            var data = await _context.Rooms.ToListAsync();
            HomeVM homeVm = new HomeVM
            {
                Rooms = data.Take(4).ToList(),
                Blogs = (await _blogService.GetAll()).Take(3).ToList(),
                Comments = (await _commentService.GetAll()).Take(5).ToList(),
            };
            return View(homeVm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReservationVM reservationVm)
        {
            // * Filter
            if (DateTime.Compare(DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture)),
                    DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture))) > 0)
            {
                ModelState.AddModelError(nameof(reservationVm.CheckIn),
                    "Check in date cannot be greater than check out date!");
                var data = await _context.Rooms.ToListAsync();
                HomeVM homeVm = new HomeVM
                {
                    Rooms = data.Take(4).ToList(),
                    Blogs = await _blogService.GetAll(),
                    Comments = (await _commentService.GetAll()).Take(5).ToList(),
                };
                return View(homeVm);
            }
            
            if (DateTime.Compare(DateTime.Now,
                    DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture))) > 0
                || DateTime.Compare(DateTime.Now,
                    DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture))) > 0
               )
            {
                ModelState.AddModelError("", "Date cannot be less than our day");
                var data = await _context.Rooms.ToListAsync();
                HomeVM homeVm = new HomeVM
                {
                    Rooms = data.Take(4).ToList(),
                    Blogs = await _blogService.GetAll(),
                    Comments = (await _commentService.GetAll()).Take(5).ToList(),
                };
                return View(homeVm);
            }

            var datas = await _context.Reservations.Where(res => !res.IsCancelled).ToListAsync();
            var filteredData = datas.Where(res =>
                (DateTime.Compare(
                     DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture)),
                     DateTime.Parse(res.CheckIn.ToString(CultureInfo.InvariantCulture))) > 0 &&
                 DateTime.Compare(
                     DateTime.Parse(res.CheckOut.ToString(CultureInfo.InvariantCulture)),
                     DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture))) > 0
                ) ||
                (DateTime.Compare(
                     DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture)),
                     DateTime.Parse(res.CheckIn.ToString(CultureInfo.InvariantCulture))) > 0 &&
                 DateTime.Compare(
                     DateTime.Parse(res.CheckOut.ToString(CultureInfo.InvariantCulture)),
                     DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture))) > 0
                ) ||
                (DateTime.Compare(
                     DateTime.Parse(res.CheckIn.ToString(CultureInfo.InvariantCulture)),
                     DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture))) > 0 &&
                 DateTime.Compare(
                     DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture)),
                     DateTime.Parse(res.CheckOut.ToString(CultureInfo.InvariantCulture))) > 0
                )
            ).ToList();
            // * End Filter
            
            var reservRooms = new List<Room>();

            foreach (var reservation in filteredData)
            {
                var room = await _roomService.Get(reservation.RoomId);
                reservRooms.Add(room);
            }
            
            var availabilRoom = (await _roomService.GetAll()).ToList()
                .Where(room => reservRooms.All(rsRoom => room.Id != rsRoom.Id));

            availabilRoom = availabilRoom.Where(rsr =>
                rsr.Capacity >= reservationVm.GuestCount && rsr.Count >= reservationVm.RoomCount).ToList();
            TempData["avRooms"] = JsonSerializer.Serialize(availabilRoom);
            return RedirectToAction("SearchResult");
        }
        
        public  IActionResult SearchResult()
        {
            List<Room> rooms = JsonSerializer.Deserialize<List<Room>>((string)TempData["avRooms"]);
            return View(rooms);
        }
    }
}