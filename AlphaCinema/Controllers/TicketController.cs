using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class TicketController : BaseController
    {
        public IActionResult Available(int id)
        {
            return View();
        }
    }
}
