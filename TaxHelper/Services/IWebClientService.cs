using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxHelper.Services
{
    internal interface IWebClientService
    {
        double GetExchangeRate(string curreccyCode, DateTime rateDate);
    }
}
