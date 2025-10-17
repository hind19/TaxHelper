namespace TaxHelper.Shared
{
    public static class Constants
    {
        public const int RoundAccuracy = 2;
        public const int MonthsBehind = -3;

        public static class ErrorConstants
        {
            public const string NoPaymentsError = "Добавьте платежи для расчета";

            public const string IncorrectPaymentFilling =
                "Суммы должны быть больше 0 и валюта платежа должна быть выбрана";
        }
    }
}
