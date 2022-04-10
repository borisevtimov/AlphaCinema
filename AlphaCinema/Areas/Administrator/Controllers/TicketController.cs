using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class TicketController : BaseController
    {
        public async Task<IActionResult> All(int id)
        {
            return View();
        }
    }
}
