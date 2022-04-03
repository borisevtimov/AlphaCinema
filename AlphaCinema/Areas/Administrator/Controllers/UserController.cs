using AlphaCinema.Core.Constants;
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

        public async Task<IActionResult> Delete(string Id) 
        {
            try
            {
                if (await adminUserService.DeleteUserAsync(Id))
                {
                    ViewData[MessageConstant.SuccessMessage] = "Deleted user successfully!";
                }
                else
                {
                    ViewData[MessageConstant.WarningMessage] = "User not found!";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            return RedirectToAction("All");
        }

        public async Task<IActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "User"
            });

            return Ok();
        }
    }
}
