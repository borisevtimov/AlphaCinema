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
        private readonly ILogger<TicketController> logger;

        public TicketController
            (
            ITicketService ticketService,
            IMovieService movieService,
            ILogger<TicketController> logger
            )
        {
            this.ticketService = ticketService;
            this.movieService = movieService;
            this.logger = logger;
        }

        public async Task<IActionResult> All(int id)
        {
            try
            {
                AdminTicketsVM model = await ticketService.GetTicketsByMovieIdAsync(id);
                return View(model);
            }
            catch (Exception e)
            {
                ViewData[ViewConstant.Title] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction("Info", "Movie", new { area = RoleConstant.User });
            }
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
                logger.LogWarning(ae.Message);
                return View();
            }
            catch (Exception e)
            {
                ViewData[ViewConstant.Title] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return View();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdminAddTicket model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    logger.LogWarning(error.ErrorMessage);
                }

                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.InvalidInput;
                return RedirectToAction(nameof(Add), new { id = model.MovieId });
            }

            try
            {
                await ticketService.AddTicketAsync(model);
            }
            catch (ArgumentException ae)
            {
                ViewData[ViewConstant.Title] = ae.Message;
                logger.LogWarning(ae.Message);
                return RedirectToAction(nameof(Add), new { id = model.MovieId });
            }
            catch (Exception e)
            {
                ViewData[ViewConstant.Title] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction(nameof(Add), new { id = model.MovieId });
            }

            return RedirectToAction(nameof(All), new { id = model.MovieId });
        }
    }
}
