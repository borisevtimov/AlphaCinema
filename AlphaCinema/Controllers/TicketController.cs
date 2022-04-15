using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IVoucherService voucherService;
        private readonly ICardService cardService;
        private readonly ILogger<TicketController> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketController(
            ITicketService ticketService,
            IVoucherService voucherService,
            ICardService cardService,
            ILogger<TicketController> logger,
            UserManager<ApplicationUser> userManager
            )
        {
            this.ticketService = ticketService;
            this.voucherService = voucherService;
            this.cardService = cardService;
            this.logger = logger;
            this.userManager = userManager;
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

        public async Task<IActionResult> Mine()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            IList<UserTicketsVM> model = await ticketService.GetTicketsForUserAsync(user);

            return View(model);
        }

        public async Task<IActionResult> Purchase(int id, string voucherCode)
        {
            SubmitPaymentVM? model = await ticketService.GetTicketInfoAsync(id);

            if (voucherCode != null)
            {
                try
                {
                    model = await voucherService.ActivateVoucherAsync(model, voucherCode);
                    return View(model);
                }
                catch (ArgumentException ae)
                {
                    ViewData[MessageConstant.WarningMessage] = ae.Message;
                    logger.LogInformation(ae.Message);
                    return RedirectToAction(nameof(Purchase), new { id = model.TicketId });
                }
                catch (Exception e)
                {
                    ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                    logger.LogInformation(e.Message);
                    return RedirectToAction(nameof(Purchase), new { id = model.TicketId });
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(SubmitPaymentVM model)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                await ticketService.PurchaseTicketAsync(user, model);
                return RedirectToAction(nameof(Mine));
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                logger.LogWarning(ae.Message);
                return RedirectToAction(nameof(Purchase), new { id = model.TicketId });
            }
            catch (InvalidOperationException oe)
            {
                ViewData[MessageConstant.ErrorMessage] = oe.Message;
                logger.LogInformation(oe.Message);
                return RedirectToAction(nameof(Purchase), new { id = model.TicketId });
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogInformation(e.Message);
                return RedirectToAction(nameof(Purchase), new { id = model.TicketId });
            }
        }
    }
}
