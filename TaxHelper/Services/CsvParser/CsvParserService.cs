using System.Configuration;
using System.IO;
using TaxHelper.Models;
using TaxHelper.Services.PaymentCreation;
using TaxHelper.Shared;

namespace TaxHelper.Services.CsvParser
{
    public class CsvParserService : ICsvParserService
    {
        private readonly IPaymentModelService _paymentModelService;
        public CsvParserService()
        {
            _paymentModelService = DependencyResolver.Resolve<IPaymentModelService>();
        }
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
