using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class VoucherController : BaseController
    {
        private readonly IVoucherService voucherService;
        private readonly ILogger<VoucherController> logger;

        public VoucherController(IVoucherService voucherService, ILogger<VoucherController> logger)
        {
            this.voucherService = voucherService;
            this.logger = logger;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                IList<DisplayVoucherVM> model = await voucherService.GetAllVouchersAsync();
                return View(model);
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVoucherVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    logger.LogWarning(error.ErrorMessage);
                }

                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.InvalidInput;
                return RedirectToAction(nameof(Create));
            }

            try
            {
                await voucherService.CreateVoucherAsync(model);
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                logger.LogWarning(ae.Message);
                return RedirectToAction(nameof(Create));
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = e.Message;
                logger.LogWarning(e.Message);
                return RedirectToAction(nameof(Create));
            }

            return RedirectToAction(nameof(All));
        }
    }
}
