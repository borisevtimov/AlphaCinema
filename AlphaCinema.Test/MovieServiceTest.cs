using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.Services;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AlphaCinema.Test
{
    public class MovieServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(s => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddSingleton<IMovieService, MovieService>()
                .BuildServiceProvider();

            var repository = serviceProvider.GetService<IRepository>();

            await SeedDatabaseAsync(repository);
        }

        [Test]
        public void AddMovieThrowsIfDateIsInvalid()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            AddMovieVM movie = new AddMovieVM
            {
                ReleaseDate = "12.16.2022"
            };

            Assert.CatchAsync<ArgumentException>(async () => await movieService.AddMovieAsync(movie),
                "Invalid date");
        }

        [Test]
        public void AddMovieAddsSuccessfully()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            AddMovieVM movie = new AddMovieVM
            {
                Description = "Test",
                Duration = 180,
                IsActive = true,
                Name = "Tarzan",
                Rating = 3.1,
                ReleaseDate = "16.04.2022"
            };

            Assert.DoesNotThrowAsync(async () => await movieService.AddMovieAsync(movie));
        }

        [Test]
        public void EditMovieThrowsIfMovieDoesNotExist()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            EditMovieVM movie = new EditMovieVM
            {
                MovieId = 2
            };

            Assert.CatchAsync<ArgumentException>(async () => await movieService.EditMovieAsync(movie),
                ExceptionConstant.MovieNotFound);
        }

        [Test]
        public void EditMovieThrowsIfReleaseDateIsInvalid()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            EditMovieVM movie = new EditMovieVM
            {
                MovieId = 1,
                ReleaseDate = "07.13.2022"
            };

            Assert.CatchAsync<ArgumentException>(async () => await movieService.EditMovieAsync(movie),
                "Invalid date");
        }

        [Test]
        public void EditMoviePasses()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            EditMovieVM movie = new EditMovieVM
            {
                MovieId = 1,
                Description = "Test",
                Duration = 50,
                IsActive = true,
                Name = "Lion King",
                Rating = 7.7,
                ReleaseDate = "07.12.2022"
            };

            Assert.DoesNotThrowAsync(async () => await movieService.EditMovieAsync(movie));
        }

        [Test]
        public void GetAllActiveMoviesMainInfoReturnsActiveMovies()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            Assert.That(() => movieService.GetAllActiveMoviesMainInfoAsync().Result.Count == 1);
        }

        [Test]
        public void GetAllMoviesMainInfoReturnsAllMovies()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            Assert.That(() => movieService.GetAllMoviesMainInfoAsync().Result.Count == 1);
        }

        [Test]
        public void GetMovieByIdThrowsIfMovieIdNotFound()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            int movieId = int.MaxValue;

            Assert.ThrowsAsync<ArgumentException>(async () => await movieService.GetMovieByIdAsync(movieId),
                ExceptionConstant.MovieNotFound);
        }

        [Test]
        public void GetMovieByIdReturnsCorrectMovie()
        {
            var movieService = serviceProvider.GetService<IMovieService>();

            int movieId = 1;

            Assert.That(() => movieService.GetMovieByIdAsync(movieId).Result.MovieId == movieId);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDatabaseAsync(IRepository repository)
        {
            Movie movie = new Movie
            {
                Id = 1,
                Description = "Test",
                Duration = 50,
                IsActive = true,
                Name = "Lion King",
                Rating = 7.7,
                ReleaseDate = new DateTime(2022, 03, 15)
            };

            await repository.AddAsync(movie);
            await repository.SaveChangesAsync();
        }
    }
}
