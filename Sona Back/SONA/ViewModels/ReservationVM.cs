using System;
using System.ComponentModel.DataAnnotations;

namespace SONA.ViewModels
{
    public class ReservationVM
    {
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public int RoomCount { get; set; }
    }
}