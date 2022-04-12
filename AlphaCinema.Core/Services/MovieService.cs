using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AlphaCinema.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository repository;

        public MovieService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddMovieAsync(AddMovieVM model)
        {
            Movie movie = new Movie()
            {
                Name = model.Name,
                Duration = model.Duration,
                Description = model.Description,
                IsActive = model.IsActive,
                Rating = model.Rating
            };

            DateTime date = DateTime.UtcNow;
            bool isParsed = DateTime.TryParseExact(model.ReleaseDate,
                FormatConstant.FullDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                throw new ArgumentException("Invalid date");
            }

            movie.ReleaseDate = date;

            await repository.AddAsync(movie);
            await repository.SaveChangesAsync();
        }

        public async Task EditMovieAsync(EditMovieVM model)
        {
            Movie? movie = await repository.All<Movie>()
                .SingleOrDefaultAsync(m => m.Id == model.MovieId);

            if (movie == null)
            {
                throw new ArgumentException(ExceptionConstant.MovieNotFound);
            }

            DateTime date = DateTime.UtcNow;
            bool isParsed = DateTime.TryParseExact(model.ReleaseDate,
                FormatConstant.FullDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                throw new ArgumentException("Invalid date");
            }

            movie.Duration = model.Duration;
            movie.Rating = model.Rating;
            movie.Description = model.Description;
            movie.IsActive = model.IsActive;
            movie.Name = model.Name;
            movie.ReleaseDate = date;

            await repository.SaveChangesAsync();
        }

        public async Task<IList<ActiveMovieMainInfoVM>> GetAllActiveMoviesMainInfoAsync()
        {
            return await repository.All<Movie>()
                .Where(m => m.IsActive == true)
                .Select(m => new ActiveMovieMainInfoVM()
                {
                    MovieId = m.Id,
                    Name = m.Name,
                    Rating = m.Rating,
                    ReleaseDate = m.ReleaseDate
                })
                .ToListAsync();
        }

        public async Task<IList<MovieMainInfoVM>> GetAllMoviesMainInfoAsync()
        {
            return await repository.All<Movie>()
                .Select(m => new MovieMainInfoVM()
                {
                    MovieId = m.Id,
                    Name = m.Name,
                    Rating = m.Rating,
                    Status = m.IsActive
                })
                .ToListAsync();
        }

        public async Task<MoreMovieInfoVM> GetMovieByIdAsync(int movieId)
        {
            Movie? movie = await repository.All<Movie>()
                .SingleOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException(ExceptionConstant.MovieNotFound);
            }

            MoreMovieInfoVM moreMovieInfoVM = new MoreMovieInfoVM()
            {
                Description = movie.Description,
                Duration = movie.Duration,
                IsActive = movie.IsActive,
                MovieId = movie.Id,
                Name = movie.Name,
                Rating = movie.Rating,
                ReleaseDate = movie.ReleaseDate
            };

            return moreMovieInfoVM;
        }

        public async Task<EditMovieVM> GetMovieForEditByIdAsync(int movieId)
        {
            Movie? movie = await repository.All<Movie>()
                .SingleOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException(ExceptionConstant.MovieNotFound);
            }

            EditMovieVM editMovieInfoVM = new EditMovieVM()
            {
                Description = movie.Description,
                Duration = movie.Duration,
                IsActive = movie.IsActive,
                MovieId = movie.Id,
                Name = movie.Name,
                ReleaseDate = Convert.ToString(string.Format("{0:dd.MM.yyyy}", movie.ReleaseDate))
            };

            return editMovieInfoVM;
        }

        public async Task<MovieFullInfoVM> GetMovieFullInfoByIdAsync(int movieId)
        {
            Movie? movie = await repository.All<Movie>()
                .SingleOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException(ExceptionConstant.MovieNotFound);
            }

            MovieFullInfoVM fullMovieInfoVM = new MovieFullInfoVM()
            {
                Description = movie.Description,
                Duration = movie.Duration,
                Name = movie.Name,
                Rating = movie.Rating,
                ReleaseDate = movie.ReleaseDate
            };

            return fullMovieInfoVM;
        }

        public async Task<string> GetMovieNameByIdAsync(int movieId)
        {
            Movie? movie = await repository.All<Movie>()
                .SingleOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException(ExceptionConstant.MovieNotFound);
            }

            return movie.Name;
        }
    }
}
