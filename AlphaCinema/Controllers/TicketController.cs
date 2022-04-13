using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly ILogger<TicketController> logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            this.ticketService = ticketService;
            this.logger = logger;
        }

        public async Task<IActionResult> Available(int id)
        {
            try
            {
                ActiveTicketsListVM activeTickets = await ticketService.GetActiveTicketsByMovieIdAsync(id);
                return View(activeTickets);
            }
            catch (InvalidOperationException oe)
            {
                ViewData[MessageConstant.WarningMessage] = oe.Message;
                logger.LogInformation(oe.Message);
                return RedirectToAction("Active", "Movie");
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogInformation(e.Message);
                return RedirectToAction("Active", "Movie");
            }
        }

        public IActionResult Purchase(int id)
        {
            return View();
        }
    }
}
