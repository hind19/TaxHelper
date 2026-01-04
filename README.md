TaxHelper

A lightweight WPF (.NET 8) utility that helps calculate personal tax amounts from income payments, with support for CSV import and automatic currency conversion using the National Bank of Ukraine (NBU) exchange rates API.

Contents
- Overview
- Features
- Requirements
- Getting started
  - Build and run
  - Configuration (App.config)
  - CSV import format
- Usage
- Architecture overview
- Troubleshooting
- FAQ
- Contributing
- License

Overview
TaxHelper enables you to:
- Enter income payments manually or import them from a CSV file.
- Convert foreign currency amounts to UAH using daily exchange rates from NBU (or a configurable API).
- Calculate taxes based on configured tax rates.

Features
- WPF desktop app using MVVM
- Manual payment entry
- CSV import with configurable column mapping and delimiter
- Automatic currency conversion (per payment date)
- Configurable tax and military tax rates
- Simple dependency container for services

Requirements
- Windows with .NET 8 SDK
- IDE: JetBrains Rider, Visual Studio 2022, or VS Code (with C# extensions)

Getting started
Build and run
1) Open TaxHelper.sln with your IDE.
2) Ensure the startup project is TaxHelper.
3) Build and run the application (Debug/Any CPU is fine).

Configuration (App.config)
Application behavior is controlled via App.config located at TaxHelper/App.config. Defaults are provided and can be adjusted to match your data source and tax rules.

Currency exchange API
- BankCurrenceExchangeUrl: Base URL of the exchange rate API.
  - Default: https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange
- CurrencyCodeParam: Query parameter name for currency code.
  - Default: valcode
- DateParam: Query parameter name for date.
  - Default: date
- AdditionalParams: Additional fixed parameters appended to every request (semicolon-separated; either key or key=value).
  - Default: json

The application builds a GET request like:
{BankCurrenceExchangeUrl}?{CurrencyCodeParam}=USD&{DateParam}=yyyyMMdd&json

If any required parameter is missing or empty, the app will report: "Параметы для запроса курсов валюты не настроены. См.ReadMe файл для дополнительной информации."

Tax rates (percent values)
- TaxRate: e.g., 5
- MilitaryTaxRate: e.g., 1

CSV import mapping
- PaymentDateColumn: CSV header name holding the payment date (e.g., DOC_DATE)
- PaymentAmountColumn: CSV header name holding the amount (e.g., CR)
- PaymentCurrencyColumn: CSV header name holding the currency code/number (e.g., CUR_NUMB)
- ColumnSplitter: Delimiter character (e.g., ;)

CSV import format
- First row must contain headers. The names must match the configured keys above.
- Example (with default settings):
  - Header: DOC_DATE;CR;CUR_NUMB
  - Row: 2024-12-30;100.50;USD

Usage
- Manual input
  - Add a payment using "+" or the Add control.
  - Enter PaymentDate, PaymentSum, and select PaymentCurrency.
  - When you change amount or currency, the app fetches the exchange rate for the selected date and recomputes PaymentSumUah.
- CSV import
  - Click Import CSV and choose a file with the structure described above.
  - The app validates headers and loads payments into the grid.
- Calculate tax
  - Click Calculate to compute taxes for all listed payments.
  - If any payment is invalid (sum <= 0 or currency not selected), the app will show an error.

Architecture overview
- MVVM
  - MainWindowVM: ViewModel driving the main screen. Holds Payments, orchestrates currency fetch and tax calculation.
- Services (resolved via a lightweight DependencyResolver)
  - TaxCalculation
    - ITaxCalculatorService, TaxCalculatorService: Aggregates payments and applies TaxRate and MilitaryTaxRate.
  - CurrencyCourse
    - IWebClientService, WebClientService: Calls exchange-rate API with parameters from App.config and returns the rate for a given currency and date.
  - CsvParser
    - ICsvParserService, CsvParserService: Reads CSV, maps columns per App.config, and produces PaymentModel instances.
  - PaymentCreation
    - IPaymentModelService, PaymentModelService: Creates PaymentModel objects from parsed CSV rows.
- Shared
  - DependencyResolver: Simple service locator used at startup (MainWindow.xaml.cs) to register implementations.
- Models
  - PaymentModel: Represents a single income payment (sum, currency, date, derived UAH sum).
  - TaxResultModel: Holds calculated totals.
- Enums
  - CurrenciesEnum: Supported currency codes including UAH.

Key flows
- Manual edit/import triggers recomputation of PaymentSumUah via WebClientService unless currency is UAH.
- Calculate command validates inputs and invokes ITaxCalculatorService to produce TaxesResult.

Troubleshooting
- Exchange rate errors
  - Verify App.config keys: BankCurrenceExchangeUrl, CurrencyCodeParam, DateParam, AdditionalParams.
  - Ensure the API endpoint is reachable and returns JSON compatible with NBU format: an array of objects containing "rate".
  - Check date format: the app uses yyyyMMdd (e.g., 20250117).
- CSV import errors
  - "Файл пустой или содержит только заголовки." — file must contain at least one data row.
  - "Файл не содержит необходимых заголовков…" — ensure headers exactly match configured names.
  - Verify ColumnSplitter matches your file’s delimiter.
- Validation errors during calculation
  - Ensure all PaymentSum values are greater than 0 and a PaymentCurrency is selected.

FAQ
- Which currencies are supported?
  - Those defined in CurrenciesEnum. For non-UAH, the app requests an exchange rate from the configured API.
- Can I use a different exchange API?
  - Yes. Update App.config keys to match your API’s base URL and parameter names. The service expects a JSON array where the first object includes a numeric "rate" field.
- How are dates formatted for the API?
  - yyyyMMdd, generated from the payment’s PaymentDate.

Contributing
- Fork the repository, create a feature branch, and open a pull request.
- Keep changes minimal and focused. Include brief notes in the PR description.

License
- No licences from my side. Feel free to use it any way you want. Please kkep in mind that some frameworks or libraries included in this solution may become proprietary any time.

P.S. File has been created by Junie (JetBrains Raider Ai Agent)
