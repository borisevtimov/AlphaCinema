using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;

namespace AlphaCinema.Core.Contracts
{
    public interface ICardService
    {
        Task<IList<DisplayCardVM>> GetAllCardsForDisplayAsync(ApplicationUser user);

        Task AddPaymentMethodAsync(ApplicationUser user, AddPaymentMethodVM model);

        Task RemovePaymentMethodAsync(string cardId);

        Task<bool> IsPaymentMethodValid(ApplicationUser user, string cardNumber);

    }
}
