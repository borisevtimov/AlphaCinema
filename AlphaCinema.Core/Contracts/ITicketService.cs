using AlphaCinema.Core.ViewModels;

namespace AlphaCinema.Core.Contracts
{
    public interface ITicketService
    {
        Task<IList<AdminTicketVM>> GetTicketsByMovieIdAsync(int movieId);

        Task AddTicketAsync(AdminAddTicket model);
    }
}
