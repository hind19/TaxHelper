using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using TaxHelper.Common;
using TaxHelper.Models;

namespace TaxHelper.Services
{
    public class CsvParserService : ICsvParserService
    {
        public IEnumerable<PaymentModel> ParseCsv(string filePath)
        {
            
            var lines = File.ReadAllLines(filePath);

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

            var importedPayments = new ObservableCollection<PaymentModel>();
            foreach (var line in lines.Skip(1))
            {
                var columns = line.Split(ConfigurationManager.AppSettings["ColumnSplitter"]);

                var paymentSum = double.Parse(columns[sumIndex], CultureInfo.InvariantCulture);
                if (paymentSum == 0)
                {
                    continue;
                }

                var payment = new PaymentModel
                {
                    PaymentDate = DateTime.Parse(columns[dateIndex]),
                    PaymentSum = paymentSum,
                    PaymentCurrency = Enum.Parse<CurrenciesEnum>(columns[currencyIndex])
                };

                importedPayments.Add(payment);
            }

            return importedPayments;
        }
    }
}
