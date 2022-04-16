using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.Services;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AlphaCinema.Test
{
    public class CardServiceTest
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
                .AddSingleton<ICardService, CardService>()
                .BuildServiceProvider();

            var repository = serviceProvider.GetService<IRepository>();

            await SeedDatabaseAsync(repository);
        }

        [Test]
        public void RemoveInvalidCardMustThrow()
        {
            var service = serviceProvider.GetService<ICardService>();

            Card card = new Card
            {
                Id = "abcd",
                Number = "1234-1234-1234-1234",
                Balance = 100,
                CVC = "563",
                ExpireDate = DateTime.MaxValue,
                UserId = "abc"
            };

            Assert.CatchAsync<ArgumentException>(async () => await service.RemovePaymentMethodAsync(card.Id), ExceptionConstant.PaymentMethodNotFound);
        }

        [Test]
        public void RemoveCardSuccessfullyRemovesCard()
        {
            var service = serviceProvider.GetService<ICardService>();
            string cardId = "abc";

            Assert.DoesNotThrowAsync(async () => await service.RemovePaymentMethodAsync(cardId));
        }

        [Test]
        public void GetAllCardsForDisplayThrowsForInvalidUser()
        {
            var service = serviceProvider.GetService<ICardService>();

            ApplicationUser? user = null;

            Assert.CatchAsync<ArgumentException>(() => service.GetAllCardsForDisplayAsync(user), ExceptionConstant.UserNotFound);

        }

        [Test]
        public void GetAllCardsForDisplayGetsCards()
        {
            var service = serviceProvider.GetService<ICardService>();

            ApplicationUser? user = new ApplicationUser()
            {
                Id = "abc"
            };

            Assert.DoesNotThrowAsync(async () => await service.GetAllCardsForDisplayAsync(user));
            Assert.That(() => service.GetAllCardsForDisplayAsync(user).Result.Count == 1);

        }

        [Test]
        public void AddPaymentMethodThrowsIfCardExists()
        {
            var service = serviceProvider.GetService<ICardService>();

            ApplicationUser? user = new ApplicationUser()
            {
                Id = "abc"
            };

            AddPaymentMethodVM model = new AddPaymentMethodVM
            {
                Number = "1234-1234-1234-1234"
            };

            Assert.CatchAsync<InvalidOperationException>(async () => await service.AddPaymentMethodAsync(user, model),
                ExceptionConstant.PaymentMethodAlreadyExists);
        }

        [Test]
        public void AddPaymentMethodThrowsIfExpireDateIsInvalid()
        {
            var service = serviceProvider.GetService<ICardService>();

            ApplicationUser? user = new ApplicationUser()
            {
                Id = "abc"
            };

            AddPaymentMethodVM model = new AddPaymentMethodVM
            {
                Number = "5555-1234-1234-1234",
                ExpireDate = "13/22"
            };

            Assert.CatchAsync<ArgumentException>(async () => await service.AddPaymentMethodAsync(user, model),
                ExceptionConstant.InvalidDate);
        }

        [Test]
        public void AddPaymentMethodThrowsIfExpireDateIsBeforeToday()
        {
            var service = serviceProvider.GetService<ICardService>();

            ApplicationUser? user = new ApplicationUser()
            {
                Id = "abc"
            };

            AddPaymentMethodVM model = new AddPaymentMethodVM
            {
                Number = "5555-1234-1234-1234",
                ExpireDate = "03/22"
            };

            Assert.CatchAsync<ArgumentException>(async () => await service.AddPaymentMethodAsync(user, model),
                ExceptionConstant.DateIsBeforeCurrent);
        }

        [Test]
        public void AddPaymentMethodAddsMethodSuccessfully()
        {
            var service = serviceProvider.GetService<ICardService>();

            ApplicationUser? user = new ApplicationUser()
            {
                Id = "abc"
            };

            AddPaymentMethodVM model = new AddPaymentMethodVM
            {
                Number = "5555-1234-1234-1234",
                ExpireDate = "06/22"
            };

            Assert.DoesNotThrow(() => service.AddPaymentMethodAsync(user, model));
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
                Number = "1234-1234-1234-1234",
                Balance = 100,
                CVC = "563",
                ExpireDate = DateTime.MaxValue,
                UserId = "abc"
            };
            await repository.AddAsync(user);
            await repository.AddAsync(card);
            await repository.SaveChangesAsync();
        }
    }
}