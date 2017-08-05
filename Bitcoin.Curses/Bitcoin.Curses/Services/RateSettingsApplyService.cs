using Bitcoin.Curses.Models;
using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services
{
    public class RateSettingsApplyService : IRateSettingsApplyService
    {
        public void SetExchangeRateVisibleOnLiveTile(string exchangeRateKey, bool isVisible)
        {
            Settings.SetLiveTileVisibility(exchangeRateKey, isVisible);
        }

        public void SetExchangeRateCurrencySymbolOnStart(string exchangeRateKey, bool isOnStart)
        {
            Settings.SetCurrencySymbolOnStartForCurrency(exchangeRateKey, isOnStart);
        }

        public void ApplySettingsToModels(IDictionary<string, BitcoinExchangeRate> models)
        {
            if (models != null)
            {
                foreach (var rate in models)
                {
                    rate.Value.IsVisibleOnLiveTile = Settings.IsLiveTileVisibility(rate.Key);
                    rate.Value.IsCurrencySymbolOnStart = Settings.IsCurrencySymbolOnStartForCurrency(rate.Key);
                }
            }
        }
    }
}