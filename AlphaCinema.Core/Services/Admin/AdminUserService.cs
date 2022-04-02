using AlphaCinema.Core.Contracts.Admin;
using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Common;
using AlphaCinema.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Core.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRepository repository;

        public AdminUserService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<AdminUserVM>> GetAllUsersAsync()
        {
            return await repository.All<ApplicationUser>()
                .Select(u => new AdminUserVM()
                {
                    Email = u.Email,
                    Id = u.Id
                })
                .ToListAsync();
        }
    }
}
