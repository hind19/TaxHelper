namespace TaxHelper.Services
{
    internal interface IWebClientService
    {
        Task<double> GetExchangeRate(string curreccyCode, DateTime rateDate);
    }
}
