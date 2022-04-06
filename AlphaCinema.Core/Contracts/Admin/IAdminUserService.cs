using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;

namespace AlphaCinema.Core.Contracts.Admin
{
    public interface IAdminUserService
    {
        Task<List<AdminUserVM>> GetAllUsersAsync();

        Task<bool> DeleteUserAsync(string userId);

        Task<IList<string>> GetUserRolesAsync(string userId);

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task AddToRolesAsync(ApplicationUser user, string[] roleIds);
    }
}
