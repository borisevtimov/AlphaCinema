using AlphaCinema.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
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

        public IActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Get(string id)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }
    }
}
