using AlphaCinema.Core.ViewModels;

namespace AlphaCinema.Core.Contracts
{
    public interface IMovieService
    {
        Task<IList<MovieMainInfoVM>> GetAllMoviesMainInfoAsync();

        Task<IList<ActiveMovieMainInfoVM>> GetAllActiveMoviesMainInfoAsync();

        Task AddMovieAsync(AddMovieVM model);

        Task<MoreMovieInfoVM> GetMovieByIdAsync(int movieId);

        Task<MovieFullInfoVM> GetMovieFullInfoByIdAsync(int movieId);

        Task<EditMovieVM> GetMovieForEditByIdAsync(int movieId);

        Task EditMovieAsync(EditMovieVM model);

        Task<string> GetMovieNameByIdAsync(int movieId);
    }
}
