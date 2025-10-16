using System.Configuration;
using TaxHelper.Models;

namespace TaxHelper.Services
{
    internal class TaxCalculatorService : ITaxCalculatorService
    {

        public TaxCalculatorService()
        {
        }
       
        public  TaxResultModel CalculateTax(IEnumerable<PaymentModel> payments)
        {
            var totalSumUah =  payments.Sum(x => x.PaymentSumUah);
            var taxRatesucceeded = Int32.TryParse(ConfigurationManager.AppSettings["TaxRate"], out int taxRate);
            var militaryTaxRatesucceeded = Int32.TryParse(ConfigurationManager.AppSettings["MilitaryTaxRate"], out int militaryTaxRate);

            if(!taxRatesucceeded || !militaryTaxRatesucceeded)
            {
                throw new ConfigurationErrorsException("Tax rates are not configured correctly.");
            }

            return new TaxResultModel
            {
                TotalSumUah = Math.Round(totalSumUah, 2),
                UnitedTax = Math.Round(totalSumUah * taxRate / 100, 2),
                MilitaryTax = Math.Round(totalSumUah * militaryTaxRate / 100, 2)
            };

        }
    }
}
