using System.Configuration;
using System.IO;
using TaxHelper.Models;
using TaxHelper.Services.PaymentCreation;
using TaxHelper.Shared;

namespace TaxHelper.Services.CsvParser
{
    /// <summary>
    /// Parses CSV files into a sequence of PaymentModel instances using column mappings
    /// defined in App.config.
    /// </summary>
    public class CsvParserService : ICsvParserService
    {
        private readonly IPaymentModelService _paymentModelService;
        /// <summary>
        /// Initializes a new instance of CsvParserService and resolves dependencies.
        /// </summary>
        public CsvParserService()
        {
            _paymentModelService = DependencyResolver.Resolve<IPaymentModelService>();
        }
        /// <summary>
        /// Parses the provided CSV file path into payment models.
        /// </summary>
        /// <param name="filePath">Absolute path to a CSV file.</param>
        /// <returns>Collection of parsed PaymentModel records.</returns>
        /// <exception cref="Exception">Thrown when the file is empty or required headers are missing.</exception>
        public async Task<IEnumerable<PaymentModel>> ParseCsvAsync(string filePath)
        {
            
            var lines = await File.ReadAllLinesAsync(filePath);

            if (lines.Length < 2)
            {
                throw new Exception("Файл пустой или содержит только заголовки.");
            }

            var headers = lines[0].Split(ConfigurationManager.AppSettings["ColumnSplitter"]);

            var dateIndex = Array.IndexOf(headers, ConfigurationManager.AppSettings["PaymentDateColumn"]);
            var sumIndex = Array.IndexOf(headers, ConfigurationManager.AppSettings["PaymentAmountColumn"]);
            var currencyIndex = Array.IndexOf(headers, ConfigurationManager.AppSettings["PaymentCurrencyColumn"]);

            if (dateIndex == -1 || sumIndex == -1 || currencyIndex == -1)
            {
                throw new Exception("Файл не содержит необходимых заголовков: PaymentDate, PaymentSum, PaymentCurrency.");
            }

            return  await _paymentModelService.GetPaymentModels( new ParsedCsvModel(lines, sumIndex, dateIndex, currencyIndex));
        }
    }
}
