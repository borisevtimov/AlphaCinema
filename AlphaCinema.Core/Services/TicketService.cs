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
    public class TicketService : ITicketService
    {
        private readonly IRepository repository;
        private readonly IMovieService movieService;
        private readonly IVoucherService voucherService;

        public TicketService(IRepository repository, IMovieService movieService, IVoucherService voucherService)
        {
            this.repository = repository;
            this.movieService = movieService;
            this.voucherService = voucherService;
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
            };

            DateTime date = DateTime.UtcNow;
            bool isParsed = DateTime.TryParseExact(model.Start,
                FormatConstant.FullDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (!isParsed || date < DateTime.Now)
            {
                throw new ArgumentException(ExceptionConstant.InvalidDate);
            }

            ticket.Start = date;

            await repository.AddAsync(ticket);
            await repository.SaveChangesAsync();
        }

        public async Task<ActiveTicketsListVM> GetActiveTicketsByMovieIdAsync(int movieId)
        {
            ActiveTicketsListVM? activeTickets = await repository.All<Ticket>()
                .Where(t => t.MovieId == movieId && t.IsPurchased == false)
                .Select(ticket => new ActiveTicketsListVM()
                {
                    MovieName = ticket.Movie.Name,
                    Tickets = ticket.Movie.Tickets.Where(t => t.IsPurchased == false)
                    .Select(t => new ActiveTicketVM()
                    {
                        TicketId = t.Id,
                        Start = t.Start,
                        Column = t.Column,
                        HallNumber = t.HallNumber,
                        Price = t.Price,
                        Row = t.Row
                    })
                       .ToList()
                })
                .FirstOrDefaultAsync();

            if (activeTickets == null)
            {
                throw new InvalidOperationException(ExceptionConstant.NotActiveTickets);
            }

            return activeTickets;
        }

        public async Task<SubmitPaymentVM> GetTicketInfoAsync(int ticketId)
        {
            SubmitPaymentVM? payment = await repository.All<Ticket>()
                .Where(t => t.Id == ticketId)
                .Select(t => new SubmitPaymentVM()
                {
                    TicketId = ticketId,
                    Column = t.Column,
                    HallNumber = t.HallNumber,
                    MovieId = t.MovieId,
                    Row = t.Row,
                    Start = t.Start,
                    PrimaryPrice = t.Price
                })
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);

            if (payment != null)
            {
                payment.MovieName = await movieService.GetMovieNameByIdAsync(payment.MovieId);
            }
            else
            {
                throw new ArgumentException(ExceptionConstant.TicketNotFound);
            }

            return payment;
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

        public async Task<IList<UserTicketsVM>> GetTicketsForUserAsync(ApplicationUser user)
        {
            IList<UserTicketsVM>? result = await repository.All<Purchase>()
                .Where(p => p.UserId == user.Id)
                .Select(p => new UserTicketsVM()
                {
                    Start = p.Ticket.Start,
                    Column = p.Ticket.Column,
                    HallNumber = p.Ticket.HallNumber,
                    Row = p.Ticket.Row,
                    MovieName = p.Ticket.Movie.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task PurchaseTicketAsync(ApplicationUser user, SubmitPaymentVM model)
        {
            Ticket? ticket = await repository.All<Ticket>()
                .FirstOrDefaultAsync(t => t.Id == model.TicketId);

            if (ticket == null)
            {
                throw new ArgumentException(ExceptionConstant.TicketNotFound);
            }

            ticket.IsPurchased = true;
            ticket.VoucherCode = model.VoucherCode;
            ticket.Price = model.FinalPrice;

            UserVoucher? userVoucher = null;

            if (model.VoucherCode != null)
            {
                userVoucher = await repository.All<UserVoucher>()
                    .FirstOrDefaultAsync(uv => uv.VoucherCode == model.VoucherCode && uv.UserId == user.Id);
            }

            Card? card = await repository.All<Card>()
                .FirstOrDefaultAsync(c => c.Number == model.CardNumber && c.UserId == user.Id);

            Purchase purchase = new Purchase
            {
                Card = card,
                PurchaseDate = DateTime.Now,
                ApplicationUser = user,
                Ticket = ticket,
                Amount = model.FinalPrice
            };

            card.Balance -= model.FinalPrice;

            repository.Delete(userVoucher);
            await repository.AddAsync(purchase);
            await repository.SaveChangesAsync();
        }
    }
}
