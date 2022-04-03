using AlphaCinema.Core.ViewModels;

namespace AlphaCinema.Core.Contracts.Admin
{
    public interface IAdminUserService
    {
        Task<List<AdminUserVM>> GetAllUsersAsync();

        Task<bool> DeleteUserAsync(string userId);

    }
}
