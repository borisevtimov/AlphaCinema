using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class MovieMainInfoVM
    {
        public int MovieId { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; } = 0;

        public bool Status { get; set; }

    }
}
