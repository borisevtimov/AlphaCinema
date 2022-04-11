using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class AdminTicketsVM
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public string MovieName { get; set; }

        public IList<AdminTicketVM> Tickets { get; set; }
    }
}
