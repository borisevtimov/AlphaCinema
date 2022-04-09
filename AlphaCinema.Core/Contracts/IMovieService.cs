using AlphaCinema.Core.ViewModels;

namespace AlphaCinema.Core.Contracts
{
    public interface IMovieService
    {
        Task<IList<MovieMainInfoVM>> GetAllMoviesMainInfoAsync();

        Task AddMovieAsync(AddMovieVM model);
    }
}
