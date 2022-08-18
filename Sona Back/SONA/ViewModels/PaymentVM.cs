using System.ComponentModel.DataAnnotations;

namespace SONA.ViewModels
{
    public class PaymentVM
    {
        [Required]
        public string CardNumber { get; set; }
        [Required] 
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string CardCvc { get; set; }
        [Required] 
        public string CardName { get; set; }
    }
}