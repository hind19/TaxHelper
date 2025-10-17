using TaxHelper.Models;

namespace TaxHelper.Services.PaymentCreation
{
    /// <summary>
    /// Creates PaymentModel instances from parsed data.
    /// </summary>
    public interface IPaymentModelService
    {
        /// <summary>
        /// Converts parsed CSV data into a sequence of PaymentModel objects.
        /// </summary>
        /// <param name="dataModel">Parsed CSV data with header indexes.</param>
        /// <returns>Collection of PaymentModel objects.</returns>
        Task<IEnumerable<PaymentModel>> GetPaymentModels(ParsedCsvModel dataModel);
    }
}
