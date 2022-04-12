using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class EditMovieVM
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; }

        [Required]
        [Range(10, 500, ErrorMessage = "Duration must be between 10 and 500 minutes!")]
        public ushort Duration { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        [RegularExpression(@"^[0-9]{2}\.[0-9]{2}\.[0-9]{4}$", ErrorMessage = "Date must be in format dd.mm.yyyy")]
        public string ReleaseDate { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(0, 10, ErrorMessage = "Rating must be number between 0 and 10")]
        public double Rating { get; set; }

        public bool IsActive { get; set; }
    }
}
