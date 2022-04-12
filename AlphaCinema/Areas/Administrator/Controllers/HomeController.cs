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
            ViewData[MessageConstant.SuccessMessage] = "Welcome to ADMIN area!";

            return View();
        }

        public IActionResult UserArea()
        {
            return RedirectToAction("Logout", "User", new { area = "" });
        }
    }
}
