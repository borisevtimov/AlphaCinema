namespace AlphaCinema.Core.ViewModels
{
    public class SubmitPaymentVM
    {
        public int TicketId { get; set; }

        public int MovieId { get; set; }

        public string MovieName { get; set; }

        public int HallNumber { get; set; }

        public byte Row { get; set; }

        public byte Column { get; set; }

        public DateTime Start { get; set; }

        public decimal PrimaryPrice { get; set; }

        public decimal VoucherDiscount { get; set; }

        public decimal FinalPrice { get; set; }

        public string VoucherCode { get; set; }

        public string CardNumber { get; set; }
    }
}
