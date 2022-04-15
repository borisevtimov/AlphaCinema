using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class CardController : BaseController
    {
        private readonly ICardService cardService;
        private readonly ILogger<CardController> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public CardController(
            ICardService cardService,
            ILogger<CardController> logger,
            UserManager<ApplicationUser> userManager
            )
        {
            this.cardService = cardService;
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                IList<DisplayCardVM> model = await cardService.GetAllCardsForDisplayAsync(user);
                return View(model);
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                logger.LogWarning(ae.Message);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPaymentMethodVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    logger.LogWarning(error.ErrorMessage);
                }

                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.InvalidInput;
                return View(nameof(Add));
            }

            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                await cardService.AddPaymentMethodAsync(user, model);
                return RedirectToAction(nameof(All));
            }
            catch (InvalidOperationException oe)
            {
                ViewData[MessageConstant.WarningMessage] = oe.Message;
                logger.LogInformation(oe.Message);
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.WarningMessage] = ae.Message;
                logger.LogInformation(ae.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        public async Task<IActionResult> Remove(string id)
        {
            try
            {
                await cardService.RemovePaymentMethodAsync(id);
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                logger.LogWarning(ae.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction(nameof(All));
            }
        }
    }
}
