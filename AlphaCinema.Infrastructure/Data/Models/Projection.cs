using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Projection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

        [Required]
        [Range(1, 20)]
        public byte HallNumber { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public Movie Movie { get; set; }
    }
}
