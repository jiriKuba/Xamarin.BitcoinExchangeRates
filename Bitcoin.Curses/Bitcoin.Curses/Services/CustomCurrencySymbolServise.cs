using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitcoin.Curses.Models;

namespace Bitcoin.Curses.Services
{
    public class CustomCurrencySymbolServise : ICustomCurrencySymbolServise
    {
        public void AddCustomCurrencySymbolToModel(IDictionary<string, BitcoinExchangeRate> models)
        {
            if (models != null)
            {
                foreach (var rate in models)
                {
                    rate.Value.CustomCurrencySymbol = Settings.GetCustomCurrencySymbol(rate.Key);
                }
            }
        }

        public void SetExchangeRateCustomCurrencySymbol(string exchangeRateKey, string customSymbol)
        {
            Settings.SetCustomCurrencySymbol(exchangeRateKey, customSymbol);
        }
    }
}