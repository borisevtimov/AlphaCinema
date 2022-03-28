using AlphaCinema.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class UserVoucher
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        [ForeignKey(nameof(Code))]
        public string VoucherCode { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Voucher Code { get; set; }
    }
}
