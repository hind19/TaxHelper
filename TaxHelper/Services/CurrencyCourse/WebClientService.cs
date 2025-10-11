using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace TaxHelper.Services
{
    internal class WebClientService : IWebClientService
    {
        public async Task<double> GetExchangeRate(string curreccyCode, DateTime rateDate)
        {
            //var url = new Uri($"{ConfigurationManager.AppSettings["BankCurrenceExchangeUrl"]}?{ConfigurationManager.AppSettings["CurrencyCodeParam"]}={curreccyCode}&{ConfigurationManager.AppSettings["DateParam"]}={rateDate.Year}{(rateDate.Month < 10 ? "0"+rateDate.Month : rateDate.Month )}{(rateDate.Day < 10 ? "0"+rateDate.Day : rateDate.Day) }&json");

            // Базовый URL
            string baseUrl = ConfigurationManager.AppSettings["BankCurrenceExchangeUrl"];

            // Параметры запроса
            Dictionary<string, string> queryParams = AssembleQueryParams(curreccyCode, rateDate);


            // Создаем HttpClient
            using var client = new HttpClient();

            // Формируем строку запроса
            var uriBuilder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }
            uriBuilder.Query = query.ToString();

            // Выполняем GET-запрос
            var response = await client.GetAsync(uriBuilder.Uri);



            //var client = new HttpClient();
            //var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var jsonArray = JsonSerializer.Deserialize<JsonArray>(content);
                if (jsonArray != null && jsonArray.Count > 0)
                {
                    var jsonObject = jsonArray[0]?.AsObject();
                    if (jsonObject?["rate"] != null)
                    {
                        return jsonObject["rate"]!.AsValue().GetValue<double>();
                    }
                }
            }
            else
            {
                throw new Exception($"Error fetching exchange rate: {response.StatusCode}");
            }
            return default(double);
        }

        private Dictionary<string, string> AssembleQueryParams(string curreccyCode, DateTime rateDate)
        {
            var queryParamsNames = new Dictionary<string, string>
            {
                { "CurrencyCodeParam", ConfigurationManager.AppSettings["CurrencyCodeParam"] },
                { "DateParam", ConfigurationManager.AppSettings["DateParam"] },
            };

            var queryParams = new Dictionary<string, string>
            {
                { queryParamsNames["CurrencyCodeParam"], curreccyCode },
                { queryParamsNames["DateParam"], $"{rateDate.Year}{(rateDate.Month < 10 ? "0" + rateDate.Month : rateDate.Month)}{(rateDate.Day < 10 ? "0" + rateDate.Day : rateDate.Day)}" }
            };

            var additionalparams = ConfigurationManager.AppSettings["AdditionalParams"];
            if (!string.IsNullOrEmpty(additionalparams))
            {
                var additionalParamsArray = additionalparams.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var param in additionalParamsArray)
                {
                    var keyValue = param.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        queryParams.Add(keyValue[0], keyValue[1]);
                    }
                    if (keyValue.Length == 1)
                    {
                        queryParams.Add(keyValue[0], string.Empty);
                    }
                }
            }

            return queryParams;
        }
    }
}
