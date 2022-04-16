using AlphaCinema.Core.ViewModels;
using AlphaCinema.Infrastructure.Data.Identity;

namespace AlphaCinema.Core.Contracts
{
    public interface IVoucherService
    {
        Task<IList<DisplayVoucherVM>> GetAllVouchersAsync();

        Task CreateVoucherAsync(CreateVoucherVM model);

        Task GetVoucherAsync(ApplicationUser user, string voucherCode);

        Task<IList<DisplayVoucherVM>> GetAllVouchersForUserAsync();

        Task<IList<DisplayVoucherVM>> GetAllUserVouchersAsync(ApplicationUser user);

        Task<SubmitPaymentVM> ActivateVoucherAsync(SubmitPaymentVM payment, string voucherCode);

        Task<SubmitVoucherVM> GetVoucherForUserByCodeAsync(ApplicationUser user, string voucherCode);
    }
}
