using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 30)]
        public byte Row { get; set; }

        [Required]
        [Range(1, 40)]
        public byte Column { get; set; }

        [Required]
        [ForeignKey(nameof(Projection))]
        public int ProjectionId { get; set; }

        [Required]
        public bool IsPurchased { get; set; }

        [Required]
        public Projection Projection { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public Ticket()
        {
            Purchases = new HashSet<Purchase>();
        }
    }
}
