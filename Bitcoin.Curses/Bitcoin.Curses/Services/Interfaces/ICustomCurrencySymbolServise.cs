using Bitcoin.Curses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services.Interfaces
{
    public interface ICustomCurrencySymbolService
    {
        void SetExchangeRateCustomCurrencySymbol(string exchangeRateKey, string customSymbol);

        void AddCustomCurrencySymbolToModel(IDictionary<string, BitcoinExchangeRate> models);
    }
}