using Jellyfish;
using TaxHelper.Common;

namespace TaxHelper.Models
{
    public class PaymentModel : ObservableObject
    {
        private double _paymentSumUah;
       
        public DateTime PaymentDate { get; set; }
        
        public double PaymentSum { get; set; }

        public CurrenciesEnum PaymentCurrency { get; set; }
        
        public double PaymentSumUah 
        {
            get
            {
                return _paymentSumUah;
            }
            set
            {
                Set(ref _paymentSumUah, value);
            }
        }
    }
}
