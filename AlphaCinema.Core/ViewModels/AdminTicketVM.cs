using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class AdminTicketVM
    {
        public int TicketId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public string MovieName { get; set; }

        [Required]
        [Range(1, 30)]
        public int HallNumber { get; set; }

        [Required]
        [Range(1, 30)]
        public byte Row { get; set; }

        [Required]
        [Range(1, 40)]
        public byte Column { get; set; }

        [Required]
        public DateTime Start { get; set; }

        public string? VoucherCode { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal Price { get; set; }

        [Required]
        public bool IsPurchased { get; set; }
    }
}
