using AlphaCinema.Core.Constants;
using AlphaCinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserArea()
        {
            return RedirectToAction("Logout", "Home", new { area = "" });
        }
    }
}
