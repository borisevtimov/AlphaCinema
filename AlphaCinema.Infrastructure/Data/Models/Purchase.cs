using AlphaCinema.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Purchase
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public Ticket Ticket { get; set; }

        [Required]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
