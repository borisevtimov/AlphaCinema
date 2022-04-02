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
    }
}
