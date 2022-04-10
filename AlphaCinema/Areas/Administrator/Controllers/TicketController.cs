using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IMovieService movieService;

        public TicketController(ITicketService ticketService, IMovieService movieService)
        {
            this.ticketService = ticketService;
            this.movieService = movieService;
        }

        public async Task<IActionResult> All(int id)
        {
            IList<AdminTicketVM> model = await ticketService.GetTicketsByMovieIdAsync(id);

            return View(model);
        }

        public async Task<IActionResult> Add(int id)
        {
            AdminAddTicket model = new AdminAddTicket()
            {
                MovieId = id
            };

            try
            {
                model.MovieName = await movieService.GetMovieNameByIdAsync(id);
            }
            catch (ArgumentException ae)
            {
                ViewData[ViewConstant.Title] = ae.Message;
                return View();
            }
            catch (Exception)
            {
                ViewData[ViewConstant.Title] = ExceptionConstant.UnexpectedError;
                return View();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(AdminAddTicket model)
        {
            ticketService.AddTicketAsync(model);

            return RedirectToAction(nameof(All));
        }
    }
}
