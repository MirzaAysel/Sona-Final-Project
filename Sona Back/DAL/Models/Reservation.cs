using System;
using Core.Entity;

namespace DAL.Models
{
    public class Reservation: IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public int RoomCount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public bool IsCancelled { get; set; }
    }
}