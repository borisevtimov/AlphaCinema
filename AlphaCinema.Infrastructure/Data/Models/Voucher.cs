using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaCinema.Infrastructure.Data.Models
{
    public class Voucher
    {
        [Key]
        [RegularExpression(@"^[A-Z0-9]{6}$")]
        [StringLength(6)]
        public string Code { get; set; }

        [Required]
        [Range(0, 1)]
        public decimal Discount { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ExpireDate { get; set; }

        public ICollection<UserVoucher> UserVouchers { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public Voucher()
        {
            UserVouchers = new HashSet<UserVoucher>();
            Tickets = new HashSet<Ticket>();
        }
    }
}
