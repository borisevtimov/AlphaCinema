namespace AlphaCinema.Core.ViewModels
{
    public class MoreMovieInfoVM
    {
        public int MovieId { get; set; }
        
        public string Name { get; set; }

        public ushort Duration { get; set; }

        public string? Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public double Rating { get; set; } = 0;

        public bool IsActive { get; set; }
    }
}
