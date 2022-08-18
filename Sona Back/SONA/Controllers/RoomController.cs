#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SONA.ViewModels;
using X.PagedList;

namespace SONA.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ICommentService _commentService;

        public RoomController(IRoomService roomService, UserManager<AppUser> userManager, AppDbContext context,
            ICommentService commentService)
        {
            _roomService = roomService;
            _userManager = userManager;
            _context = context;
            _commentService = commentService;
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
                    statusCode = 504,
                    message = e.Message
                });
            }

            return View(rooms.ToPagedList(page, 6));
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

            ViewBag.Data = room;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ReservationVM reservationVm, int id)
        {
            // * Filter
            if (DateTime.Compare(DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture)),
                    DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture))) > 0)
            {
                ModelState.AddModelError(nameof(reservationVm.CheckIn),
                    "Check in date cannot be greater than check out date!");
                ViewBag.Data = await _roomService.Get(id);
                RoomDetailsVM roomDetailsVm = new RoomDetailsVM
                {
                    ReservationVm = reservationVm
                };
                return View(roomDetailsVm);
            }

            if (DateTime.Compare(DateTime.Now,
                    DateTime.Parse(reservationVm.CheckIn.ToString(CultureInfo.InvariantCulture))) > 0
                || DateTime.Compare(DateTime.Now,
                    DateTime.Parse(reservationVm.CheckOut.ToString(CultureInfo.InvariantCulture))) > 0
               )
            {
                ModelState.AddModelError("", "Date cannot be less than our day");
                ViewBag.Data = await _roomService.Get(id);
                RoomDetailsVM roomDetailsVm = new RoomDetailsVM
                {
                    ReservationVm = reservationVm
                };
                return View(roomDetailsVm);
            }

            var data = await _context.Reservations.Where(res => res.RoomId == id && !res.IsCancelled).ToListAsync();
            var filteredData = data.Where(res =>
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

            if (filteredData.Count > 0)
            {
                ModelState.AddModelError("", "You cannot make reservations during this period.");
                ViewBag.Data = await _roomService.Get(id);
                RoomDetailsVM roomDetailsVm = new RoomDetailsVM
                {
                    ReservationVm = reservationVm
                };
                return View(roomDetailsVm);
            }
            // * Filter End

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            Reservation reservation = new Reservation
            {
                CheckIn = reservationVm.CheckIn,
                CheckOut = reservationVm.CheckOut,
                CreatedDate = DateTime.Now,
                GuestCount = reservationVm.GuestCount,
                RoomCount = reservationVm.RoomCount,
                RoomId = id,
                UserId = user.Id,
                IsPaid = false,
                IsCancelled = true
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Checkout", reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(Reservation reservation)
        {
            RoomVM roomVm = new RoomVM
            {
                Reservation = reservation,
                Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == reservation.RoomId)
            };

            ViewBag.Data = roomVm;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Checkout(PaymentVM paymentVm, int id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

            if (!ModelState.IsValid)
            {
                RoomVM roomVm = new RoomVM
                {
                    Reservation = reservation,
                    Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == reservation.RoomId)
                };

                ViewBag.Data = roomVm;
                return View(paymentVm);
            }

            reservation.IsPaid = true;
            reservation.IsCancelled = false;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "MyRooms");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComment(Comment comment, int id)
        {
            if (comment.FullName is null || comment.Email is null || comment.Message is null)
            {
                return RedirectToAction("Details",new {id});
            }
            
            try
            {
                comment.CreatedDate = DateTime.Now;
                comment.UpdatedDate = DateTime.Now;
                await _commentService.Create(comment);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = 504,
                    message = e.Message
                });
            }
            
            return RedirectToAction("Details", "Room", new { id = id, });
        }
    }
}