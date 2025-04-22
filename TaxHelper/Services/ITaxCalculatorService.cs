using TaxHelper.Models;

namespace TaxHelper.Services
{
    internal interface ITaxCalculatorService 
    {
        Task<TaxResultModel> CalculateTax(IEnumerable<PaymentModel> payments);
    }
}
