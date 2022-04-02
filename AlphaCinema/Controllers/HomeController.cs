using AlphaCinema.Core.Constants;
using AlphaCinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlphaCinema.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //ViewData[MessageConstant.ErrorMessage] = "Error, something went wrong!";
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login() 
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Logout()
        {
            SignOut();

            return RedirectToAction("Index");
        }
    }
}