using Jellyfish;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaxHelper.Models;
using TaxHelper.Services;
using TaxHelper.Services.CsvParser;
using TaxHelper.Services.CurrencyCourse;
using TaxHelper.Shared;

namespace TaxHelper
{
    /// <summary>
    /// ViewModel for the main window. Manages payments, currency conversion, and tax calculation.
    /// </summary>
    public class MainWindowVM : ViewModel
    {
        #region Fields
        private ObservableCollection<PaymentModel> _payments;
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ICsvParserService _csvParserService;
        private readonly IWebClientService _webClientService;
        private TaxResultModel? _taxesResult;
        private DataSourceType _selectedDataSource;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the MainWindowVM and resolves required services.
        /// </summary>
        public MainWindowVM()
        {
            SelectedDataSource = DataSourceType.Manual;
            _payments = new ObservableCollection<PaymentModel>();
            CurrenciesList = Enum.GetValues(typeof(CurrenciesEnum)).Cast<CurrenciesEnum>().ToList();
            _taxCalculatorService = DependencyResolver.Resolve<ITaxCalculatorService>();
            _csvParserService = DependencyResolver.Resolve<ICsvParserService>();
            _webClientService = DependencyResolver.Resolve<IWebClientService>();
        }
        #endregion

        #region Properties
        public DataSourceType SelectedDataSource
        {
            get => _selectedDataSource;
            set => Set(ref _selectedDataSource, value);
        }

        public ObservableCollection<PaymentModel> Payments 
        {
            get { return _payments; }
            set => Set(ref _payments, value);
        }

        public TaxResultModel? TaxesResult
        {
            get { return _taxesResult; }
            set { _taxesResult = value; }
        }

        public List<CurrenciesEnum> CurrenciesList { get; }
        #endregion

        #region Comamnds
        /// <summary>
        /// Adds a new payment row with a pre-filled date based on the configured MonthsBehind value.
        /// </summary>
        public RelayCommand AddPaymentCommand => new RelayCommand((obj) =>
        {
            var payment = new PaymentModel { PaymentDate = DateTime.Now.AddMonths(Constants.MonthsBehind) };
            Payments.Add(payment);
        });

        /// <summary>
        /// Removes the specified payment row when the command parameter is a PaymentModel.
        /// </summary>
        public RelayCommand RemovePaymentCommand => new RelayCommand((obj) =>
        {
              if (obj is PaymentModel payment)
              {
                  Payments.Remove(payment);
              }
        });
        
        /// <summary>
        /// Recalculates the UAH amount for a payment when amount or currency changes; fetches exchange rate if needed.
        /// </summary>
        public RelayCommand RecalculateSumCommand => new RelayCommand(async (obj) =>
        {
            var args = obj as RoutedEventArgs;
            if (args is null)
            {
                return;
            }

            if (args.OriginalSource is TextBox textBox
                &&  textBox.DataContext is PaymentModel payment)
            {
                await RecalculatePaymentData(payment);
            }

            if (args.OriginalSource is ComboBox comboBox
               && comboBox.DataContext is PaymentModel paymentCur)
            {
                await RecalculatePaymentData(paymentCur);
            }

            async Task RecalculatePaymentData(PaymentModel payment)
            {
                if (payment.PaymentCurrency != default && payment.PaymentSum != default)
                {
                    if (payment.PaymentCurrency == CurrenciesEnum.UAH)
                    {
                        payment.PaymentSumUah = payment.PaymentSum;
                        return;
                    }

                    var exchangeRate = await _webClientService.GetExchangeRate(payment.PaymentCurrency.ToString(), payment.PaymentDate);
                    payment.PaymentSumUah = Math.Round(payment.PaymentSum * exchangeRate, Constants.RoundAccuracy);
                }
            }
        });

        /// <summary>
        /// Validates payments and calculates taxes for all listed payments.
        /// </summary>
        public RelayCommand CalculateTaxCommand => new RelayCommand((obj) =>
        {
            
            if (Payments.Any(p => p.PaymentSum <= 0 || !Enum.TryParse(p.PaymentCurrency.ToString(), out CurrenciesEnum paymentCurrency)))
            {
                MessageBox.Show("Суммы должны быть больше 0 и валюта платежа должна быть выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TaxesResult = _taxCalculatorService.CalculateTax(Payments);
            Notify(nameof(TaxesResult));
            
        });

        /// <summary>
        /// Opens a file dialog and imports payments from a CSV file using configured mappings.
        /// </summary>
        public RelayCommand ImportCsvCommand => new RelayCommand(async (obj) =>
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
                   
                    Payments = new ObservableCollection<PaymentModel>(await _csvParserService.ParseCsvAsync(filePath));
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
