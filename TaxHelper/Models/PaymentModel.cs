using TaxHelper.Common;

namespace TaxHelper.Models
{
    public class PaymentModel
    {
        public DateTime PaymentDate { get; set; }
        public double PaymentSum { get; set; }
        public Currencies PaymentCurrency { get; set; }
    }
}
