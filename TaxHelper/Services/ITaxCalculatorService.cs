using TaxHelper.Models;

namespace TaxHelper.Services
{
    internal interface ITaxCalculatorService 
    {
        TaxResultModel CalculateTax(IEnumerable<PaymentModel> payments);
    }
}
