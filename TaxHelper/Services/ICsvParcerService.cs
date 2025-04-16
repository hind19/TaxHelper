using TaxHelper.Models;

namespace TaxHelper.Services
{
    public interface ICsvParserService
    {
        IEnumerable<PaymentModel> ParseCsv(string filePath);
    }
}
