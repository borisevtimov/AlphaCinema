namespace AlphaCinema.Core.ViewModels
{
    public class UserTicketsVM
    {
        public string MovieName { get; set; }

        public int HallNumber { get; set; }

        public byte Row { get; set; }

        public byte Column { get; set; }

        public DateTime Start { get; set; }
    }
}
