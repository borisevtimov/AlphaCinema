using AlphaCinema.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Card
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$")]
        public string Number { get; set; }

        [Required]
        public int CVC { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ExpireDate { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public Card()
        {
            Id = Guid.NewGuid().ToString();
            Purchases = new HashSet<Purchase>();
        }
    }
}
