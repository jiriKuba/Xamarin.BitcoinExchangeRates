using Bitcoin.Curses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services.Interfaces
{
    public interface IRateSettingsApplyService
    {
        void SetExchangeRateVisibleOnLiveTile(string exchangeRateKey, bool isVisible);

        void SetExchangeRateCurrencySymbolOnStart(string exchangeRateKey, bool isOnStart);

        void ApplySettingsToModels(IDictionary<string, BitcoinExchangeRate> models);
    }
}