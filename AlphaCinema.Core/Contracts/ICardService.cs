using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;

namespace AlphaCinema.Core.Contracts
{
    public interface ICardService
    {
        Task<IList<DisplayCardVM>> GetAllCardsForDisplayAsync(ApplicationUser user);

        Task AddPaymentMethod(ApplicationUser user, AddPaymentMethodVM model);
    }
}
