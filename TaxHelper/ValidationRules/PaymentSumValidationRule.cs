using System;
using System.Globalization;
using System.Windows.Controls;

namespace TaxHelper.ValidationRules
{
    internal class PaymentSumValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null || !decimal.TryParse(value.ToString(), NumberStyles.Any, cultureInfo, out decimal paymentSum))
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
