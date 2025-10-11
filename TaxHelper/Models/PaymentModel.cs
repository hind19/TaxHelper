using TaxHelper.Common;

namespace TaxHelper.Models
{
    public class PaymentModel
    {
        private double _paymentSum;
        private double _paymentSumUah;
        public DateTime PaymentDate { get; set; }
        public double PaymentSum {
            get
            {
                return _paymentSum;
            }
            set 
            {
                _paymentSum = _paymentSumUah = value;
            }
        } 
        public CurrenciesEnum PaymentCurrency { get; set; }
        public double PaymentSumUah { get; set; }
    }
}
