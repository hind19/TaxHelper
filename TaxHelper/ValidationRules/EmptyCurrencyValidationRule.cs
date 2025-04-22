using System.Globalization;
using System.Windows.Controls;
using TaxHelper.Common;

namespace TaxHelper.ValidationRules
{
    public class EmptyCurrencyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null || !Enum.TryParse(value.ToString(), out CurrenciesEnum paymentCurrency))
            {
                return new ValidationResult(false, "Валюта платежа не выбрана.");
            }


            return ValidationResult.ValidResult;
        }

        
    }   
 }
