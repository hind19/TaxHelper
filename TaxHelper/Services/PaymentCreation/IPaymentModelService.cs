using TaxHelper.Models;

namespace TaxHelper.Services.PaymentCreation
{
    public interface IPaymentModelService
    {
        Task<IEnumerable<PaymentModel>> GetPaymentModels(ParsedCsvModel dataModel);
    }
}
