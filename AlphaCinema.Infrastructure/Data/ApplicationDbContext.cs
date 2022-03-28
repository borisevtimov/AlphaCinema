using AlphaCinema.Infrastructure.Data.Identity;
using AlphaCinema.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<UserVoucher> UserVouchers { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Purchase>()
                .HasKey(k => new { k.UserId, k.TicketId });

            builder.Entity<Movie>()
                .Property(p => p.Rating)
                .HasDefaultValue(0);

            builder.Entity<UserVoucher>()
                .HasKey(k => new { k.UserId, k.VoucherCode });
        }
    }
}