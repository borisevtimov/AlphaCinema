using System.ComponentModel.DataAnnotations;

namespace AlphaCinema.Core.ViewModels
{
    public class DisplayVoucherVM
    {
        public string Code { get; set; }

        public decimal Discount { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpireDate { get; set; }

        public bool IsUsed { get; set; }
    }
}
