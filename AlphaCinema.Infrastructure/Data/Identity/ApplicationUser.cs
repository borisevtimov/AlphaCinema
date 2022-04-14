using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public DateTime RegisteredOn { get; set; } = DateTime.Now;

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<UserVoucher> UserVouchers { get; set; }

        public ICollection<Card> Cards { get; set; }

        public ApplicationUser()
        {
            Purchases = new HashSet<Purchase>();
            UserVouchers = new HashSet<UserVoucher>();
            Cards = new HashSet<Card>();
        }
    }
}
