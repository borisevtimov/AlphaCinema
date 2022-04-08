using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class AddMovieVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(10, 500, ErrorMessage = "Duration must be between 10 and 500 minutes!")]
        public ushort Duration { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
