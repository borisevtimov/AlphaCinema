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

        public async Task<AdminTicketsVM> GetTicketsByMovieIdAsync(int movieId)
        {
            AdminTicketsVM? ticketsModel = await repository.All<Movie>()
                .Where(m => m.Id == movieId)
                .Select(m => new AdminTicketsVM()
                {
                    MovieId = movieId,
                    MovieName = m.Name,
                    Tickets = m.Tickets.Select(t => new AdminTicketVM()
                    {
                        Start = t.Start,
                        Column = t.Column,
                        HallNumber = t.HallNumber,
                        IsPurchased = t.IsPurchased,
                        Price = t.Price,
                        Row = t.Row,
                        VoucherCode = t.VoucherCode
                    })
                       .ToList()
                })
                .FirstOrDefaultAsync();

            return ticketsModel;
        }
    }
}
