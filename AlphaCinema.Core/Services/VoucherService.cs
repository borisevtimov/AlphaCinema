using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AlphaCinema.Core.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IRepository repository;

        public VoucherService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task CreateVoucherAsync(CreateVoucherVM model)
        {
            Voucher? voucher = await repository.All<Voucher>()
                .SingleOrDefaultAsync(v => v.Code == model.Code);

            if (voucher != null)
            {
                throw new ArgumentException(ExceptionConstant.VoucherAlreadyExists);
            }

            DateTime date = DateTime.UtcNow;
            bool isParsed = DateTime.TryParseExact(model.ExpireDate,
                FormatConstant.FullDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed)
            {
                throw new ArgumentException(ExceptionConstant.InvalidDate);
            }

            Voucher resultVoucher = new Voucher()
            {
                Code = model.Code,
                Discount = model.Discount,
                ExpireDate = date.Date,
                IsUsed = false
            };

            await repository.AddAsync(resultVoucher);
            await repository.SaveChangesAsync();
        }

        public async Task<IList<DisplayVoucherVM>> GetAllVouchersAsync()
        {
            return await repository.All<Voucher>()
                .Select(v => new DisplayVoucherVM()
                {
                    Code = v.Code,
                    Discount = v.Discount,
                    ExpireDate = v.ExpireDate.Date,
                    IsUsed = v.IsUsed
                })
                .ToListAsync();
        }

        public async Task<IList<DisplayVoucherVM>> GetAllVouchersForUserAsync()
        {
            return await repository.All<Voucher>()
                .Select(v => new DisplayVoucherVM()
                {
                    Code = v.Code,
                    Discount = v.Discount,
                    ExpireDate = v.ExpireDate
                })
                .ToListAsync();
        }

        public async Task GetVoucherAsync(ApplicationUser user, string voucherCode)
        {
            if (user.UserVouchers.Any(v => v.VoucherCode == voucherCode))
            {
                throw new InvalidOperationException(ExceptionConstant.VoucherAlreadyOwned);
            }

            user.UserVouchers.Add(new UserVoucher()
            {
                UserId = user.Id,
                VoucherCode = voucherCode
            });

            await repository.SaveChangesAsync();
        }
    }
}
