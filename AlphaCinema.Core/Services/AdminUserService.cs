using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Core.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRepository repository;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminUserService(
            IRepository repository,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
            )
        {
            this.repository = repository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<List<AdminUserVM>> GetAllUsersAsync()
        {
            List<AdminUserVM> users = await repository.All<ApplicationUser>()
                .Select(u => new AdminUserVM()
                {
                    Email = u.Email,
                    Id = u.Id,
                    RegisteredOn = u.RegisteredOn,
                })
                .ToListAsync();

            foreach (AdminUserVM userVM in users)
            {
                userVM.Roles = await GetUserRolesAsync(userVM.Id);
            }

            return users;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            ApplicationUser? user = await repository.All<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            return await userManager.GetRolesAsync(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await repository.All<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            repository.Delete(user);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await repository.All<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task AddToRolesAsync(ApplicationUser user, string[] roleIds)
        {
            string[] roles = await roleManager.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();

            await userManager.AddToRolesAsync(user, roles);

            await repository.SaveChangesAsync();
        }
    }
}
