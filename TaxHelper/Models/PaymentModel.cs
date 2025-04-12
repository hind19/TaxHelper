using TaxHelper.Common;

namespace TaxHelper.Models
{
    public class PaymentModel
    {
        public DateTime PaymentDate { get; set; }
        public int PaymentSum { get; set; }
        public Currencies PaymentCurrency { get; set; }
    }
}
