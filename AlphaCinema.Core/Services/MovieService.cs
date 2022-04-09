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
                IsActive = model.IsActive
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

        public async Task<IList<MovieMainInfoVM>> GetAllMoviesMainInfoAsync()
        {
            return await repository.All<Movie>()
                .Select(m => new MovieMainInfoVM()
                {
                    Name = m.Name,
                    Rating = m.Rating,
                    Status = m.IsActive
                })
                .ToListAsync();
        }
    }
}
