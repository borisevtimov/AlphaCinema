using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.Services;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AlphaCinema.Test
{
    public class TicketServiceTest
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
                .AddSingleton<ITicketService, TicketService>()
                .AddSingleton<IMovieService, MovieService>()
                .AddSingleton<IVoucherService, VoucherService>()
                .BuildServiceProvider();

            var repository = serviceProvider.GetService<IRepository>();

            await SeedDatabaseAsync(repository);

            
        }

        [Test]
        public void AddTicketThrowsIfStartDateTimeIsInvalid()
        {
            var service = serviceProvider.GetService<ITicketService>();

            AdminAddTicket model = new AdminAddTicket()
            {
                Start = "12.04.2022 23:65:50"
            };

            Assert.CatchAsync<ArgumentException>(async () => await service.AddTicketAsync(model),
                ExceptionConstant.InvalidDate);
        }

        [Test]
        public void AddTicketAddsTicketSuccessfully()
        {
            var service = serviceProvider.GetService<ITicketService>();

            AdminAddTicket model = new AdminAddTicket()
            {
                Start = "12.04.2022 23:55:50",
                MovieId = 5
            };

            Assert.DoesNotThrowAsync(async () => await service.AddTicketAsync(model));
        }

        [Test]
        public void GetActiveTicketsByMovieIdThrowsIfThereArentActiveTickets()
        {
            var service = serviceProvider.GetService<ITicketService>();

            int movieId = 1;

            Assert.ThrowsAsync<InvalidOperationException>(async () => await
            service.GetActiveTicketsByMovieIdAsync(movieId), ExceptionConstant.NotActiveTickets);
        }

        [Test]
        public void GetActiveTicketsByMovieIdGetsActiveTickets()
        {
            var service = serviceProvider.GetService<ITicketService>();

            Movie movie = new Movie
            {
                Id = 5,
                Name = "Tarzan"
            };

            Assert.That(() => service.GetActiveTicketsByMovieIdAsync(movie.Id)
            .Result.MovieName == movie.Name);
        }

        [Test]
        public void GetTicketInfoThrowsIfTicketIdIsInvalid()
        {
            var service = serviceProvider.GetService<ITicketService>();

            int ticketId = 2;

            Assert.CatchAsync<ArgumentException>(async () => await service.GetTicketInfoAsync(ticketId),
                ExceptionConstant.TicketNotFound);
        }

        [Test]
        public void GetTicketInfoDoesNotThrow()
        {
            var service = serviceProvider.GetService<ITicketService>();

            int ticketId = 1;

            Assert.DoesNotThrowAsync(async () => await service.GetTicketInfoAsync(ticketId));
        }

        [Test]
        public void GetTicketsByMovieIdReturnsMovieWithTickets()
        {
            var service = serviceProvider.GetService<ITicketService>();

            int movieId = 5;

            Assert.DoesNotThrowAsync(async () => await service.GetTicketsByMovieIdAsync(movieId));
        }

        [Test]
        public void GetTicketsForUserReturnsListOfTickets()
        {
            var service = serviceProvider.GetService<ITicketService>();

            ApplicationUser user = new ApplicationUser
            {
                Id = "abc"
            };

            Assert.DoesNotThrowAsync(async () => await service.GetTicketsForUserAsync(user));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDatabaseAsync(IRepository repository)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "abc"
            };

            Card card = new Card
            {
                Id = "abc",
                ApplicationUser = user,
                UserId = user.Id,
                CVC = "000",
                Balance = 1000,
                ExpireDate = new DateTime(2022, 8, 10),
                Number = "0000-0000-0000-0000"
            };

            Movie movie = new Movie
            {
                Id = 5,
                Description = "Test",
                Duration = 100,
                IsActive = true,
                Name = "Tarzan",
                Rating = 6,
                ReleaseDate = new DateTime(2022, 4, 12)
            };

            Ticket ticket = new Ticket
            {
                Id = 1,
                HallNumber = 3,
                Row = 4,
                Column = 5,
                IsPurchased = false,
                Price = 10,
                Start = new DateTime(2022, 4, 20),
                Movie = movie
            };

            Purchase purchase = new Purchase
            {
                Id = "def",
                UserId = user.Id,
                Amount = 10,
                ApplicationUser = user,
                PurchaseDate = DateTime.Now,
                Ticket = ticket,
                TicketId = ticket.Id,
                Card = card,
                CardId = card.Id
            };

            await repository.AddAsync(user);
            await repository.AddAsync(card);
            await repository.AddAsync(movie);
            await repository.AddAsync(ticket);
            await repository.AddAsync(purchase);
            await repository.SaveChangesAsync();
        }
    }
}
