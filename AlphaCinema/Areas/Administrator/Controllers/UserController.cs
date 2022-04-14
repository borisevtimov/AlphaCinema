using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            try
            {
                List<AdminUserVM> users = await adminUserService.GetAllUsersAsync();

                return View(users);
            }
            catch (ArgumentException)
            {
                return View();
            }
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

        public async Task<IActionResult> ChangeRole(string Id)
        {
            ApplicationUser user = await adminUserService.GetUserByIdAsync(Id);

            UserRolesVM userRoles = new UserRolesVM()
            {
                UserId = user.Id,
                Email = user.Email
            };

            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Id,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                });

            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(UserRolesVM model)
        {
            ApplicationUser user = await adminUserService.GetUserByIdAsync(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleIds.Length > 0)
            {
                await adminUserService.AddToRolesAsync(user, model.RoleIds);
            }

            return RedirectToAction(nameof(All));
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
