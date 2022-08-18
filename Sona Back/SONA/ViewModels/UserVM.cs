using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SONA.ViewModels
{
    public class UserVM
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Image { get; set; }
        
        public IFormFile ImageFile { get; set; }
    }
}