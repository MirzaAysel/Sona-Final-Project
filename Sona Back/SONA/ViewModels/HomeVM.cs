using System.Collections.Generic;
using DAL.Models;

namespace SONA.ViewModels
{
    public class HomeVM
    {
        public ReservationVM ReservationVm { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Comment> Comments { get; set; }
    }
}