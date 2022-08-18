using System.ComponentModel.DataAnnotations;

namespace SONA.ViewModels
{
    public class ChangePasswordVM
    {
        
        [Required,DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}