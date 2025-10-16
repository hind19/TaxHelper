namespace TaxHelper.Services.CurrencyCourse
{
    internal interface IWebClientService
    {
        Task<double> GetExchangeRate(string curreccyCode, DateTime rateDate);
    }
}
