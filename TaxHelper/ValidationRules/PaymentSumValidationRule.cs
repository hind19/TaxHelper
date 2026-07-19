using System;
using System.Globalization;
using System.Windows.Controls;

namespace TaxHelper.ValidationRules
{
    internal class PaymentSumValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var normalized = value?.ToString()?.Trim().Replace(",", ".");
            if (normalized is null || !decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal paymentSum))
            {
                return new ValidationResult(false, "Строка суммы содержит невалидные символы.");
            }

            if (Math.Round(paymentSum, 2) != paymentSum)
            {
                return new ValidationResult(false, "Сумма платежа должна содержать не более 2 знаков после запятой.");
            }

            if (paymentSum <= 0)
            {
                return new ValidationResult(false, "Сумма платежа должна быть больше нуля.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
