using System.Windows;
using TaxHelper.Common;
using TaxHelper.Services;

namespace TaxHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        { 
            RegisterDependencies();
            InitializeComponent();
        }

        private void RegisterDependencies()
        {
            DependencyResolver.Register<ITaxCalculatorService>(() => new TaxCalculatorService());
            DependencyResolver.Register<IWebClientService>(() => new WebClientService());
            DependencyResolver.Register<ICsvParserService>(() => new CsvParserService());
        }
    }
}