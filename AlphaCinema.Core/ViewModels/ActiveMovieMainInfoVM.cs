namespace AlphaCinema.Core.ViewModels
{
    public class ActiveMovieMainInfoVM
    {
        public int MovieId { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; } = 0;

        public DateTime ReleaseDate { get; set; }
    }
}
