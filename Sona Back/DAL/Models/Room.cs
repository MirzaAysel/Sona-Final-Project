using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;
using DAL.Base;
using Microsoft.AspNetCore.Http;

namespace DAL.Models
{
    public class Room: BaseEntity, IEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double DiscountPrice { get; set; }
        public string Image { get; set; }
        [Required]
        public double Size { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Bed { get; set; }
        [Required]
        public string Services { get; set; }
        [Required]
        public int Count { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        
        public List<Reservation> Reservations { get; set; }
    }
}