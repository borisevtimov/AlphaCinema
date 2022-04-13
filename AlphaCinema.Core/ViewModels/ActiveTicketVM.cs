namespace AlphaCinema.Core.ViewModels
{
    public class ActiveTicketVM
    {
        public int TicketId { get; set; }

        public int HallNumber { get; set; }

        public byte Row { get; set; }

        public byte Column { get; set; }

        public DateTime Start { get; set; }

        public decimal Price { get; set; }
    }
}
