using System.ComponentModel.DataAnnotations;
using Core.Entity;
using DAL.Base;

namespace DAL.Models
{
    public class Comment : BaseEntity, IEntity
    {
        [Required]
        public string FullName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required] 
        public string Message { get; set; }
        public bool IsContact { get; set; }
    }
}