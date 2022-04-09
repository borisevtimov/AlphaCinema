﻿using AlphaCinema.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = RoleConstant.Administrator)]
        public IActionResult Administrator() 
        {
            return RedirectToAction("Index", "Home", new { area = RoleConstant.Administrator });
        }
    }
}