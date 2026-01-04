using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace TaxHelper.Services.CurrencyCourse
{
    /// <summary>
    /// HTTP client service that retrieves currency exchange rates from a configured API.
    /// Reads endpoint and query parameter names from App.config.
    /// </summary>
    internal class WebClientService : IWebClientService
    {
        /// <summary>
        /// Gets the exchange rate for a given currency code on a specific date.
        /// </summary>
        /// <param name="curreccyCode">ISO currency code, e.g., USD, EUR, UAH.</param>
        /// <param name="rateDate">The date for which the rate is requested.</param>
        /// <returns>Numeric exchange rate value. Returns 0 if rate not found.</returns>
        /// <exception cref="ArgumentException">Thrown when required App.config parameters are missing.</exception>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        /// <exception cref="Exception">Thrown when the API response indicates an error status code.</exception>
        public async Task<double> GetExchangeRate(string curreccyCode, DateTime rateDate)
        {
            string? baseUrl = ConfigurationManager.AppSettings["BankCurrenceExchangeUrl"];
            Dictionary<string, string> queryParams = AssembleQueryParams(curreccyCode, rateDate);

            if (string.IsNullOrWhiteSpace(baseUrl) 
                //|| queryParams.Any(x =>  string.IsNullOrWhiteSpace(x.Value)) // TODO: Solve the case with parameter 'json' withtout value
                || !queryParams.Any())
            {
                throw new ArgumentException("Параметы для запроса курсов валюты не настроены. См.ReadMe файл для дополнительной информации.");
            }

            using var client = new HttpClient();

            var uriBuilder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }
            uriBuilder.Query = query.ToString();

            var response = await client.GetAsync(uriBuilder.Uri);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var jsonArray = JsonSerializer.Deserialize<JsonArray>(content);
                if (jsonArray is not null && jsonArray.Count > 0)
                {
                    var jsonObject = jsonArray[0]?.AsObject();
                    if (jsonObject?["rate"] is not null)
                    {
                        return jsonObject["rate"]!.AsValue().GetValue<double>();
                    }
                }
            }
            else
            {
                throw new Exception($"Error fetching exchange rate: {response.StatusCode}");
            }
            return default;
        }

        /// <summary>
        /// Builds query string parameters for the exchange rate API using configured key names.
        /// </summary>
        /// <param name="currencyCode">Currency code to request, e.g., USD.</param>
        /// <param name="rateDate">Date for which the exchange rate is requested.</param>
        /// <returns>Dictionary of query parameter names and values.</returns>
        private Dictionary<string, string> AssembleQueryParams(string currencyCode, DateTime rateDate)
        {
            var queryParamsNames = new Dictionary<string, string>
            {
                { "CurrencyCodeParam", ConfigurationManager.AppSettings["CurrencyCodeParam"] },
                { "DateParam", ConfigurationManager.AppSettings["DateParam"] },
            };

            var queryParams = new Dictionary<string, string>
            {
                { queryParamsNames["CurrencyCodeParam"], currencyCode },
                { queryParamsNames["DateParam"],
                    $"{rateDate.Year}{(rateDate.Month < 10 ? "0" + rateDate.Month : rateDate.Month)}{(rateDate.Day < 10 ? "0" + rateDate.Day : rateDate.Day)}" }
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
