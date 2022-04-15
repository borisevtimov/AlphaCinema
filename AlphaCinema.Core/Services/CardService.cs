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
    public class CardService : ICardService
    {
        private readonly IRepository repository;

        public CardService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddPaymentMethod(ApplicationUser user, AddPaymentMethodVM model)
        {
            Card? card = await repository.All<Card>()
                .FirstOrDefaultAsync(c => c.Number == model.Number && c.UserId == user.Id);

            if (card != null)
            {
                throw new InvalidOperationException(ExceptionConstant.PaymentMethodAlreadyExists);
            }

            DateTime date = DateTime.UtcNow;
            bool isParsed = DateTime.TryParseExact(model.ExpireDate,
                FormatConstant.CardExpireDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (isParsed == false)
            {
                throw new ArgumentException(ExceptionConstant.InvalidDate);
            }

            if (date < DateTime.Now)
            {
                throw new ArgumentException(ExceptionConstant.DateIsBeforeCurrent);
            }

            Card resultCard = new Card
            {
                Number = model.Number,
                UserId = user.Id,
                Balance = model.Balance,
                ExpireDate = date,
                CVC = model.CVC
            };

            await repository.AddAsync(resultCard);
            await repository.SaveChangesAsync();
        }

        public async Task<IList<DisplayCardVM>> GetAllCardsForDisplayAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentException(ExceptionConstant.UserNotFound);
            }

            return await repository.All<Card>()
                .Where(c => c.UserId == user.Id)
                .Select(c => new DisplayCardVM()
                {
                    Id = c.Id,
                    Balance = c.Balance,
                    ExpireDate = c.ExpireDate,
                    Number = c.Number
                })
                .ToListAsync();
        }
    }
}
