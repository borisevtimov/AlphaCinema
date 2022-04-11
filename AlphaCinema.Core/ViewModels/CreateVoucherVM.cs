using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class CreateVoucherVM
    {
        [RegularExpression(@"^[A-Z0-9]{6}$", ErrorMessage = "Code must contain only 6 capital characters!")]
        [StringLength(6)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Discout is required!")]
        [Range(0, 1, ErrorMessage = "Discount must be floating point number between 0 and 1!")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Date is required!")]
        [RegularExpression(@"^[0-9]{2}\.[0-9]{2}\.[0-9]{4}$", ErrorMessage = "Date must be in format dd.mm.yyyy")]
        public string ExpireDate { get; set; }
    }
}
