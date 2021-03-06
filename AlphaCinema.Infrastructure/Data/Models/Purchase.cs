using AlphaCinema.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Purchase
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }

        [ForeignKey(nameof(Card))]
        public string? CardId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal Amount { get; set; }

        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public Ticket Ticket { get; set; }

        public Card? Card { get; set; }

        public Purchase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
