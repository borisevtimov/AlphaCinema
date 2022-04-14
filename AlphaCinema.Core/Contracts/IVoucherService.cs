using AlphaCinema.Core.ViewModels;

namespace AlphaCinema.Core.Contracts
{
    public interface IVoucherService
    {
        Task<IList<DisplayVoucherVM>> GetAllVouchersAsync();

        Task CreateVoucherAsync(CreateVoucherVM model);

        Task GetVoucher(string userId, string voucherCode);
    }
}
