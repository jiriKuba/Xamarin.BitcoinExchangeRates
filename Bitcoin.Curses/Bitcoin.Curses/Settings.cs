using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses
{
    public static class Settings
    {
        private const string EXCHANGE_RATES_LIVE_TITLE_VISIBILITY = "ExchangeRateViewModel.ShowRateInLiveTile.";
        private const string LAST_VIEWED_EXCHANGE_RATE = "LastViewedExchangeRate";
        private const string DEFAULT_EXCHANGE_RATE = "USD";

        public static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static Boolean IsLiveTileVisibility(string currencyCode)
        {
            return AppSettings.GetValueOrDefault<bool>(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, false);
        }

        public static void SetLiveTileVisibility(string currencyCode, bool isVisible)
        {
            AppSettings.AddOrUpdateValue<bool>(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, isVisible);
        }

        public static string GetLastViewedCurrency()
        {
            return AppSettings.GetValueOrDefault<string>(LAST_VIEWED_EXCHANGE_RATE, DEFAULT_EXCHANGE_RATE);
        }

        public static void SetLastViewedCurrency(string currencyCode)
        {
            AppSettings.AddOrUpdateValue<string>(LAST_VIEWED_EXCHANGE_RATE, currencyCode);
        }
    }
}
