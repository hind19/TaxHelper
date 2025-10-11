using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using TaxHelper.Common;
using TaxHelper.Models;

namespace TaxHelper.Services.PaymentCreation
{
    class PaymentModelService : IPaymentModelService
    {
        private readonly IWebClientService _webClientService;
        private const int HeaderLinesNumber = 1;
        private const int RoundAccuracy = 2;

        public PaymentModelService()
        {
            _webClientService = DependencyResolver.Resolve<IWebClientService>();
        }
        
        public async Task<IEnumerable<PaymentModel>> GetPaymentModels(ParsedCsvModel dataModel)
        {
            var importedPayments = new List<PaymentModel>();
            foreach (var line in dataModel.Lines.Skip(HeaderLinesNumber))
            {
                var columns = line.Split(ConfigurationManager.AppSettings["ColumnSplitter"]);

                var paymentSum = double.Parse(columns[dataModel.SumIndex], CultureInfo.InvariantCulture);
                if (paymentSum == 0)
                {
                    continue;
                }

                var payment = new PaymentModel
                {
                    PaymentDate = DateTime.Parse(columns[dataModel.DateIndex]),
                    PaymentSum = paymentSum,
                    PaymentCurrency = Enum.Parse<CurrenciesEnum>(columns[dataModel.CurrencyIndex]),
                    PaymentSumUah = paymentSum
                };
                if(payment.PaymentCurrency != CurrenciesEnum.UAH)
                {
                    var exchangeRate = await _webClientService.GetExchangeRate(payment.PaymentCurrency.ToString(), payment.PaymentDate);
                    payment.PaymentSumUah = Math.Round(payment.PaymentSum * exchangeRate, RoundAccuracy);
                }

                importedPayments.Add(payment);
            }
            return importedPayments.OrderBy(x=> x.PaymentDate);

        }
    }
}
