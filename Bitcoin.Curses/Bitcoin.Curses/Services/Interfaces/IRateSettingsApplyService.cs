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

        void SetLastBitcoinExchangeRates(string jsonData);

        string GetLastBitcoinExchangeRates();

        void SetLastExchangeRatesByUSD(string jsonData);

        string GetLastExchangeRatesByUSD();

        void SetLastYesterdayUSDRate(string jsonData);

        string GetLastYesterdayUSDRate();

        void SetLastSpotUSDRate(string jsonData);

        string GetLastSpotUSDRate();
    }
}