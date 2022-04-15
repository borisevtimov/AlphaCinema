using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class VoucherController : BaseController
    {
        private readonly IVoucherService voucherService;
        private readonly ILogger<VoucherController> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public VoucherController(
            IVoucherService voucherService, 
            ILogger<VoucherController> logger,
            UserManager<ApplicationUser> userManager
            )
        {
            this.voucherService = voucherService;
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Get()
        {
            IList<DisplayVoucherVM> model = await voucherService.GetAllVouchersForUserAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                await voucherService.GetVoucherAsync(user, id);
                return RedirectToAction(nameof(Mine));
            }
            catch (InvalidOperationException oe)
            {
                ViewData[MessageConstant.WarningMessage] = oe.Message;
                logger.LogInformation(oe.Message);
                return RedirectToAction(nameof(Get));
            }
        }

        public async Task<IActionResult> Mine()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            IList<DisplayVoucherVM> model = await voucherService.GetAllUserVouchersAsync(user);

            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitVoucher(SubmitPaymentVM model)
        {
            return RedirectToAction("Purchase", "Ticket", new { id = model.TicketId, voucherCode = model.VoucherCode });
        }
    }
}
