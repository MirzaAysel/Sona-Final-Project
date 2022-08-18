using System.ComponentModel.DataAnnotations;
using Core.Entity;

namespace DAL.Models
{
    public class Setting:IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}