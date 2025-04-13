using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TaxHelper.Services
{
    internal class WebClientService : IWebClientService
    {
        public double GetExchangeRate(string curreccyCode, DateTime rateDate)
        {
            var url = new Uri($"{ConfigurationManager.AppSettings["BankCurrenceExchangeUrl"]}?{ConfigurationManager.AppSettings["CurrencyCodeParam"]}={curreccyCode}&{ConfigurationManager.AppSettings["DateParam"]}={rateDate.Year}{(rateDate.Month < 10 ? "0"+rateDate.Month : rateDate.Month )}{(rateDate.Day < 10 ? "0"+rateDate.Day : rateDate.Day) }&json");

            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
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
    }
}
