using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.Services;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlphaCinema.Test
{
    public class VoucherServiceTest
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
                .AddSingleton<IVoucherService, VoucherService>()
                .BuildServiceProvider();

            var repository = serviceProvider.GetService<IRepository>();

            await SeedDatabaseAsync(repository);
        }

        [Test]
        public void ActivateVoucherThrowsIfVoucherIsNotValid()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            string voucherCode = "41XADS";
            SubmitPaymentVM payment = new SubmitPaymentVM();

            Assert.CatchAsync<ArgumentException>(async () => await service.ActivateVoucherAsync(payment, voucherCode),
                ExceptionConstant.VoucherDoesNotExist);
        }

        [Test]
        public async Task ActivateVoucherReturnsCorrectVoucher()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            string voucherCode = "CX90VC";
            SubmitPaymentVM payment = new SubmitPaymentVM()
            {
                PrimaryPrice = 10
            };

            SubmitPaymentVM resultPayment = await service.ActivateVoucherAsync(payment, voucherCode);

            Assert.AreEqual(resultPayment.VoucherDiscount, 0.35m);
            Assert.AreEqual(resultPayment.VoucherCode, voucherCode);
            Assert.AreEqual(resultPayment.FinalPrice, payment.PrimaryPrice * (1 - payment.VoucherDiscount));
        }

        [Test]
        public void CreateVoucherThrowsIfVoucherAlreadyExists()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            string voucherCode = "41XADS";
            CreateVoucherVM model = new CreateVoucherVM()
            {
                Code = voucherCode,
            };

            Assert.CatchAsync<ArgumentException>(async () => await service.CreateVoucherAsync(model),
                ExceptionConstant.VoucherAlreadyExists);
        }

        [Test]
        public void CreateVoucherThrowsIfExpireDateIsInvalid()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            string voucherCode = "CX90VC";
            CreateVoucherVM model = new CreateVoucherVM()
            {
                Code = voucherCode,
                ExpireDate = "32.12.2022"
            };

            Assert.CatchAsync<ArgumentException>(async () => await service.CreateVoucherAsync(model),
                ExceptionConstant.InvalidDate);
        }

        [Test]
        public void CreateVoucherDoesNotThrow()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            string voucherCode = "X201BV";
            CreateVoucherVM model = new CreateVoucherVM()
            {
                Code = voucherCode,
                ExpireDate = "31.12.2022",
                Discount = 0.21m
            };

            Assert.DoesNotThrowAsync(async () => await service.CreateVoucherAsync(model));
        }

        [Test]
        public void GetAllUserVouchersReturnsCorrectVouchers()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            ApplicationUser user = new ApplicationUser
            {
                Id = "abc"
            };

            Assert.That(() => service.GetAllUserVouchersAsync(user).Result.Count == 1);
        }

        [Test]
        public void GetAllVouchersReturnsDoesNotThrow()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            Assert.That(() => service.GetAllVouchersAsync().Result.Count == 1);
        }

        [Test]
        public void GetAllVouchersForUserReturnsAllVouchers()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            Assert.That(() => service.GetAllVouchersForUserAsync().Result.Count == 1);
        }

        [Test]
        public void GetVoucherThrowsIfUserAlreadyHasIt()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            ApplicationUser user = new ApplicationUser
            {
                Id = "abc",
            };

            string voucherCode = "CX90VC";

            user.UserVouchers = new List<UserVoucher>()
            {
                 new UserVoucher
                 {
                      UserId = user.Id,
                      VoucherCode = voucherCode
                 }
            };


            Assert.CatchAsync<InvalidOperationException>(async () => await
            service.GetVoucherAsync(user, voucherCode), ExceptionConstant.VoucherAlreadyOwned);
        }

        [Test]
        public void GetVoucherAddsVoucherToUser()
        {
            var service = serviceProvider.GetService<IVoucherService>();

            ApplicationUser user = new ApplicationUser
            {
                Id = "abc",
            };

            string voucherCode = "XWTZM1";

            Assert.DoesNotThrowAsync(async () => await service.GetVoucherAsync(user, voucherCode));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDatabaseAsync(IRepository repository)
        {
            Voucher voucher = new Voucher
            {
                Code = "CX90VC",
                Discount = 0.35m,
                ExpireDate = new DateTime(2022, 07, 15),
            };

            ApplicationUser user = new ApplicationUser
            {
                Id = "abc"
            };

            UserVoucher userVoucher = new UserVoucher()
            {
                UserId = user.Id,
                ApplicationUser = user,
                VoucherCode = voucher.Code,
                Code = voucher
            };

            await repository.AddAsync(voucher);
            await repository.AddAsync(user);
            await repository.AddAsync(userVoucher);
            await repository.SaveChangesAsync();
        }
    }
}
