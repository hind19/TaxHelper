namespace TaxHelper.Models
{
    internal class ExchangeRateApiModel
    {
        public int R030 { get; set; }
        public string Txt { get; set; } = string.Empty;
        public double Rate { get; set; }
        public string Cc { get; set; } = string.Empty;
        public DateTime Exchangedate { get; set; }
    }
}
