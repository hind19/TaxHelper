using TaxHelper.Models;

namespace TaxHelper.Services.CsvParser
{
    public interface ICsvParserService
    {
        Task<IEnumerable<PaymentModel>> ParseCsvAsync(string filePath);
    }
}
