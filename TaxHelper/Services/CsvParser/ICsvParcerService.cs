using TaxHelper.Models;

namespace TaxHelper.Services.CsvParser
{
    /// <summary>
    /// Defines operations for parsing CSV sources into PaymentModel sequences.
    /// </summary>
    public interface ICsvParserService
    {
        /// <summary>
        /// Parses a CSV file into a collection of PaymentModel items.
        /// </summary>
        /// <param name="filePath">Absolute path to the CSV file.</param>
        /// <returns>Parsed collection of payments.</returns>
        Task<IEnumerable<PaymentModel>> ParseCsvAsync(string filePath);
    }
}
