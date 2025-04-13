using Jellyfish;
using System.Collections.ObjectModel;
using TaxHelper.Models;
using TaxHelper.Common;
using TaxHelper.Services;



namespace TaxHelper
{
    public class MainWindowVM : ViewModel
    {
        #region Fields
       
        private bool _isManualSource;
        private bool _isCsvSource;
        private ObservableCollection<PaymentModel>? _payments;
        private readonly ITaxCalculatorService _taxCalculatorService;
        private TaxResultModel? _taxesResult;
        #endregion

        #region Ctor
        public MainWindowVM()
        {
            IsManualSource = true;
            Payments = new ObservableCollection<PaymentModel>
            {
                new PaymentModel
                {
                    PaymentDate = DateTime.Now.AddDays(-10),
                    PaymentSum = 1000,
                    PaymentCurrency = Currencies.USD
                },
                new PaymentModel
                {
                    PaymentDate = DateTime.Now.AddDays(-2),
                    PaymentSum = 2000,
                    PaymentCurrency = Currencies.EUR
                }
            };
            CurrenciesList = Enum.GetValues(typeof(Currencies)).Cast<Currencies>().ToList();
            _taxCalculatorService = DependencyResolver.Resolve<ITaxCalculatorService>();

           var taxes =  _taxCalculatorService.CalculateTax(Payments);

        }
        #endregion

        #region Properties
        public bool IsManualSource
        {
            get { return _isManualSource; }
            set => Set(ref _isManualSource, value);
        }

        public bool IsCsvSource
        {
            get { return _isCsvSource; }
            set => Set(ref _isCsvSource, value);
        }

        public ObservableCollection<PaymentModel> Payments 
        {
            get { return _payments; }
            set => Set(ref _payments, value);
        }

        public TaxResultModel TaxesResult
        {
            get { return _taxesResult; }
            set { _taxesResult = value; }
        }

        public List<Currencies> CurrenciesList { get; }
        #endregion

        #region Comamnds

        public RelayCommand AddPaymentCommand => new RelayCommand((obj) =>
        {
            var payment = new PaymentModel();
            Payments.Add(payment);

        });

        public RelayCommand RemovePaymentCommand => new RelayCommand((obj) =>
        {
            if (obj is PaymentModel payment)
            {
                Payments.Remove(payment);
            }
        });

        public RelayCommand CalculateTaxCommand => new RelayCommand((obj) =>
        {
            TaxesResult = _taxCalculatorService.CalculateTax(Payments);
            
        });

        #endregion

        #region Methods

        #endregion
    }
}
