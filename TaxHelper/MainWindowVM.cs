using Jellyfish;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Windows;
using TaxHelper.Common;
using TaxHelper.Models;
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
        private readonly ICsvParserService _csvParserService;
        private TaxResultModel? _taxesResult;
        #endregion

        #region Ctor
        public MainWindowVM()
        {
            IsManualSource = true;
            Payments = new ObservableCollection<PaymentModel>();
            CurrenciesList = Enum.GetValues(typeof(Currencies)).Cast<Currencies>().ToList();
            _taxCalculatorService = DependencyResolver.Resolve<ITaxCalculatorService>();
            _csvParserService = DependencyResolver.Resolve<ICsvParserService>();
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
            var payment = new PaymentModel { PaymentDate = DateTime.Now.AddMonths(-3) };
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
            Notify(nameof(TaxesResult));
            
        });

        public RelayCommand ImportCsvCommand => new RelayCommand((obj) =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Выберите CSV файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                   
                    Payments = new ObservableCollection<PaymentModel>(_csvParserService.ParseCsv(filePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при импорте CSV: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        });
        #endregion

        #region Methods
        #endregion
    }
}
