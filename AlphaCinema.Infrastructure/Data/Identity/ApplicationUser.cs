using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public DateTime RegisteredOn { get; set; } = DateTime.Now;
    }
}
