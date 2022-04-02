using AlphaCinema.Core.Contracts.Admin;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminUserService adminUserService;

        public UserController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IAdminUserService adminUserService
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.adminUserService = adminUserService;
        }

        public async Task<IActionResult> All()
        {
            List<AdminUserVM> users = await adminUserService.GetAllUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator"
            //});

            return Ok();
        }
    }
}
