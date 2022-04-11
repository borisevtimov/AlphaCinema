using AlphaCinema.Core.ViewModels;

namespace AlphaCinema.Core.Contracts
{
    public interface ITicketService
    {
        Task<AdminTicketsVM> GetTicketsByMovieIdAsync(int movieId);

        Task AddTicketAsync(AdminAddTicket model);
    }
}
