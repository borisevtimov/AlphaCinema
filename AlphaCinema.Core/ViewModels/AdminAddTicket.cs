using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class AdminAddTicket
    {
        public int TicketId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public string MovieName { get; set; }

        [Required]
        [Range(1, 30, ErrorMessage = "Hall number must be between 1 and 30!")]
        public int HallNumber { get; set; }

        [Required]
        [Range(1, 30, ErrorMessage = "Row number must be between 1 and 30!")]
        public byte Row { get; set; }

        [Required]
        [Range(1, 40, ErrorMessage = "Column number must be between 1 and 40!")]
        public byte Column { get; set; }

        [Required(ErrorMessage = "Start date time is required!")]
        [RegularExpression(@"^[0-9]{2}\.[0-9]{2}\.[0-9]{4} [0-9]{2}:[0-9]{2}:[0-9]{2}$", ErrorMessage = "Date must be in format dd.mm.yyyy HH:mm:ss")]
        public DateTime Start { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Price must be between 1 and 100lv.!")]
        public decimal Price { get; set; }
    }
}
