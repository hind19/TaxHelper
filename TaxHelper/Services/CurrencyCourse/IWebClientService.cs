namespace TaxHelper.Services.CurrencyCourse
{
    /// <summary>
    /// Provides access to currency exchange rates.
    /// </summary>
    internal interface IWebClientService
    {
        /// <summary>
        /// Retrieves an exchange rate for a specified currency and date.
        /// </summary>
        /// <param name="curreccyCode">ISO currency code.</param>
        /// <param name="rateDate">Date for which to retrieve the rate.</param>
        /// <returns>Exchange rate as a double.</returns>
        Task<double> GetExchangeRate(string curreccyCode, DateTime rateDate);
    }
}
