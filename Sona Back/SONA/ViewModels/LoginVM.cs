using System.ComponentModel.DataAnnotations;

namespace SONA.ViewModels
{
    public class LoginVM
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password))]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}