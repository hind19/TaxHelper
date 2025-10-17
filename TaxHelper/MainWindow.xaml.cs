using System.Windows;
using TaxHelper.Services;
using TaxHelper.Services.CsvParser;
using TaxHelper.Services.CurrencyCourse;
using TaxHelper.Services.PaymentCreation;
using TaxHelper.Shared;

namespace TaxHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes the main window and registers application dependencies.
        /// </summary>
        public MainWindow()
        { 
            RegisterDependencies();
            InitializeComponent();
        }

        /// <summary>
        /// Registers service implementations in the application's dependency resolver.
        /// </summary>
        private void RegisterDependencies()
        {
            DependencyResolver.Register<ITaxCalculatorService>(() => new TaxCalculatorService());
            DependencyResolver.Register<IWebClientService>(() => new WebClientService());
            DependencyResolver.Register<ICsvParserService>(() => new CsvParserService());
            DependencyResolver.Register<IPaymentModelService>(() => new PaymentModelService());
        }
    }
}