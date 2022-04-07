using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository repository;

        public MovieService(IRepository repository)
        {
            this.repository = repository;
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
