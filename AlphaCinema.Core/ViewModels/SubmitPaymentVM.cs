using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class SubmitPaymentVM
    {
        public int TicketId { get; set; }

        public int MovieId { get; set; }

        public string MovieName { get; set; }

        public int HallNumber { get; set; }

        public byte Row { get; set; }

        public byte Column { get; set; }

        public DateTime Start { get; set; }

        public decimal PrimaryPrice { get; set; }

        public decimal VoucherDiscount { get; set; }

        public decimal FinalPrice { get; set; }

        [RegularExpression(@"^[A-Z0-9]{6}$", ErrorMessage = "Code must contain only 6 capital characters!")]
        [StringLength(6)]
        public string VoucherCode { get; set; }

        [Required(ErrorMessage = "Card number is required!")]
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "Car number must be in format 0000-0000-0000-0000")]
        public string CardNumber { get; set; }
    }
}
