namespace AlphaCinema.Core.ViewModels
{
    public class ActiveTicketsListVM
    {
        public string MovieName { get; set; }

        public IList<ActiveTicketVM> Tickets { get; set; }
    }
}
