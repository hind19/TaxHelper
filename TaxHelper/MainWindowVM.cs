using Jellyfish;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TaxHelper.Common;
using TaxHelper.Models;
using TaxHelper.Services;

namespace TaxHelper
{
    public class MainWindowVM : ViewModel
    {
        #region Fields
        private ObservableCollection<PaymentModel>? _payments;
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ICsvParserService _csvParserService;
        private readonly IWebClientService _webClientService;
        private TaxResultModel? _taxesResult;
        private DataSourceType _selectedDataSource;
        #endregion

        #region Ctor
        public MainWindowVM()
        {
            SelectedDataSource = DataSourceType.Manual;
            Payments = new ObservableCollection<PaymentModel>();
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

        public TaxResultModel TaxesResult
        {
            get { return _taxesResult; }
            set { _taxesResult = value; }
        }

        public List<CurrenciesEnum> CurrenciesList { get; }
        #endregion

        #region Comamnds
        public RelayCommand AddPaymentCommand => new RelayCommand((obj) =>
        {
            var payment = new PaymentModel { PaymentDate = DateTime.Now.AddMonths(Constants.MonthsBehind) };
            Payments.Add(payment);
        });

        public RelayCommand RemovePaymentCommand => new RelayCommand((obj) =>
        {
              if (obj is PaymentModel payment)
              {
                  Payments.Remove(payment);
              }
        });
        
        public RelayCommand RecalculateSumCommand => new RelayCommand((obj) =>
        {
            var args = obj as RoutedEventArgs;
            if (args is null)
            {
                return;
            }

            if (args.OriginalSource is TextBox textBox
                &&  textBox.DataContext is PaymentModel payment)
            {
                RecalculatePaymentData(payment);
            }

            if (args.OriginalSource is ComboBox comboBox
               && comboBox.DataContext is PaymentModel paymentCur)
            {
                RecalculatePaymentData(paymentCur);
            }

            void RecalculatePaymentData(PaymentModel payment)
            {
                if (payment.PaymentCurrency != default && payment.PaymentSum != default)
                {
                    if (payment.PaymentCurrency == CurrenciesEnum.UAH)
                    {
                        payment.PaymentSumUah = payment.PaymentSum;
                        return;
                    }

                    var exchangeRateTask = Task.Run(() => _webClientService.GetExchangeRate(payment.PaymentCurrency.ToString(), payment.PaymentDate));
                    var exchangeRate = exchangeRateTask.Result;
                    payment.PaymentSumUah = Math.Round(payment.PaymentSum * exchangeRate, Constants.RoundAccuracy);
                }
            }
        });

        public RelayCommand CalculateTaxCommand => new RelayCommand(async (obj) =>
        {
            
            if (Payments.Any(p => p.PaymentSum <= 0 || !Enum.TryParse(p.PaymentCurrency.ToString(), out CurrenciesEnum paymentCurrency)))
            {
                MessageBox.Show("Суммы должны быть больше 0 и валюта платежа должна быть выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TaxesResult = await _taxCalculatorService.CalculateTax(Payments);
            Notify(nameof(TaxesResult));
            
        });

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
