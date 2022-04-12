using AlphaCinema.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Logout()
        {
            SignOut();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        public IActionResult Administrator()
        {
            return RedirectToAction("Index", "Home", new { area = RoleConstant.Administrator });
        }
    }
}
