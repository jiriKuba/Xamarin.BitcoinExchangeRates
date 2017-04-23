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
        private const string EXCHANGE_RATES_CUSTOM_SYMBOL = "ExchangeRateViewModel.CustomCurrencySymbol.";
        private const string LAST_VIEWED_EXCHANGE_RATE = "LastViewedExchangeRate";
        private const string DEFAULT_EXCHANGE_RATE = "USD";

        public static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static bool IsLiveTileVisibility(string currencyCode)
        {
            return AppSettings.GetValueOrDefault<bool>(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, false);
        }

        public static void SetLiveTileVisibility(string currencyCode, bool isVisible)
        {
            AppSettings.AddOrUpdateValue<bool>(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, isVisible);
        }

        public static string GetCustomCurrencySymbol(string currencyCode)
        {
            return AppSettings.GetValueOrDefault<string>(EXCHANGE_RATES_CUSTOM_SYMBOL + currencyCode, null);
        }

        public static void SetCustomCurrencySymbol(string currencyCode, string customSymbol)
        {
            AppSettings.AddOrUpdateValue<string>(EXCHANGE_RATES_CUSTOM_SYMBOL + currencyCode, customSymbol);
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