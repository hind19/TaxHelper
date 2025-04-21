using System.Globalization;
using System.Windows.Controls;

namespace TaxHelper.ValidationRules
{
    internal class PaymentSumValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null || !double.TryParse(value.ToString(), out double paymentSum))
            {
                return new ValidationResult(false, "Строка суммы содкржит невалидные символы.");
            }

            if (paymentSum <= 0)
            {
                return new ValidationResult(false, "Сумма платежа ждолжна быть больше нуля.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
