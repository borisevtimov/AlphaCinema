using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class AddPaymentMethodVM
    {
        [Required(ErrorMessage = "Card number is required!")]
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "Car number must be in format 0000-0000-0000-0000")]
        public string Number { get; set; }

        [Required(ErrorMessage = "CVC code is required!")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "CVC code must be 3 digits")]
        public string CVC { get; set; }

        [Required(ErrorMessage = "Expire date is required!")]
        [RegularExpression(@"^\d{2}\/\d{2}$", ErrorMessage = "Expire date must be in format mm/yy")]
        public string ExpireDate { get; set; }

        [Required(ErrorMessage = "Balance is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be positive number!")]
        public decimal Balance { get; set; }
    }
}
