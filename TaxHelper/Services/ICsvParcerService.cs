using TaxHelper.Models;

namespace TaxHelper.Services
{
    public interface ICsvParserService
    {
        Task<IEnumerable<PaymentModel>> ParseCsvAsync(string filePath);
    }
}
