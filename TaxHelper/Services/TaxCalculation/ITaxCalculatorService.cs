using TaxHelper.Models;

namespace TaxHelper.Services
{
    /// <summary>
    /// Calculates taxes based on provided payments and configured tax rates.
    /// </summary>
    internal interface ITaxCalculatorService 
    {
        /// <summary>
        /// Computes tax totals for the specified payment collection.
        /// </summary>
        /// <param name="payments">Sequence of payments to calculate taxes for.</param>
        /// <returns>Aggregated tax results.</returns>
        TaxResultModel CalculateTax(IEnumerable<PaymentModel> payments);
    }
}
