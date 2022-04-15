using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;

namespace AlphaCinema.Core.Contracts
{
    public interface ITicketService
    {
        Task<AdminTicketsVM> GetTicketsByMovieIdAsync(int movieId);

        Task AddTicketAsync(AdminAddTicket model);

        Task<ActiveTicketsListVM> GetActiveTicketsByMovieIdAsync(int movieId);

        Task<SubmitPaymentVM> GetTicketInfoAsync(int ticketId);

        Task PurchaseTicketAsync(ApplicationUser user, SubmitPaymentVM model);

        Task<IList<UserTicketsVM>> GetTicketsForUserAsync(ApplicationUser user);
    }
}
