using AlphaCinema.Core.Contracts.Admin;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Core.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRepository repository;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminUserService(IRepository repository, RoleManager<IdentityRole> roleManager)
        {
            this.repository = repository;
            this.roleManager = roleManager;
        }

        public async Task<List<AdminUserVM>> GetAllUsersAsync()
        {
            return await repository.All<ApplicationUser>()
                .Select(u => new AdminUserVM()
                {
                    Email = u.Email,
                    Id = u.Id,
                    RegisteredOn = u.RegisteredOn,
                })
                .ToListAsync();
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
    }
}
