using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly IRepository repository;

        public TicketService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddTicketAsync(AdminAddTicket model)
        {
            Ticket ticket = new Ticket()
            {
                Column = model.Column,
                HallNumber = model.HallNumber,
                IsPurchased = false,
                MovieId = model.MovieId,
                Price = model.Price,
                Row = model.Row,
                Start = model.Start
            };

            await repository.AddAsync(ticket);
            await repository.SaveChangesAsync();
        }

        public async Task<IList<AdminTicketVM>> GetTicketsByMovieIdAsync(int movieId)
        {
            return await repository.All<Ticket>()
                .Where(ticket => ticket.MovieId == movieId)
                .Select(ticket => new AdminTicketVM()
                {
                    MovieName = ticket.Movie.Name,
                    Column = ticket.Column,
                    HallNumber = ticket.HallNumber,
                    IsPurchased = ticket.IsPurchased,
                    Price = ticket.Price,
                    Row = ticket.Row,
                    Start = ticket.Start,
                    VoucherCode = ticket.VoucherCode,
                    MovieId = movieId
                })
                .ToListAsync();
        }
    }
}
