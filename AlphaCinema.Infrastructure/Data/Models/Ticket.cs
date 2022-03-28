using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

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

        [ForeignKey(nameof(Code))]
        public string? VoucherCode { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal Price { get; set; }

        [Required]
        public bool IsPurchased { get; set; }

        [Required]
        public Movie Movie { get; set; }

        public Voucher Code { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public Ticket()
        {
            Purchases = new HashSet<Purchase>();
        }
    }
}
