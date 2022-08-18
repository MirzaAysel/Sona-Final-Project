using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;
using DAL.Base;
using Microsoft.AspNetCore.Http;

namespace DAL.Models
{
    public class Blog: BaseEntity,IEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string Tag { get; set; }
        [Required]
        public string Author { get; set; }
    }
}