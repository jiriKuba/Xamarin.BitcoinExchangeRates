using GalaSoft.MvvmLight.Ioc;
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
        private const string EXCHANGE_RATES_CURRENCY_SYMBOL_ON_START = "ExchangeRateViewModel.IsCurrencySymbolOnStart.";
        private const string EXCHANGE_RATES_CUSTOM_SYMBOL = "ExchangeRateViewModel.CustomCurrencySymbol.";
        private const string LAST_VIEWED_EXCHANGE_RATE = "LastViewedExchangeRate";
        private const string LAST_BITCOIN_EXCHANGE_RATES = "LastBitcoinExchangeRates";
        private const string LAST_EXCHANGE_RATES_BY_USD = "LastExchangeRatesByUSD";
        private const string LAST_YESTERDAY_USD_RATE = "LastYesterdayUSDRate";
        private const string LAST_SPOT_USD_RATE = "LastSpotUSDRate";
        private const string DEFAULT_EXCHANGE_RATE = "USD";

        public static ISettings AppSettings
        {
            get
            {
                if (SimpleIoc.Default.IsRegistered<ISettings>())
                {
                    return SimpleIoc.Default.GetInstance<ISettings>();
                }
                else
                {
                    return CrossSettings.Current;
                }
            }
        }

        public static bool IsLiveTileVisibility(string currencyCode)
        {
            return AppSettings.GetValueOrDefault(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, false);
        }

        public static void SetLiveTileVisibility(string currencyCode, bool isVisible)
        {
            AppSettings.AddOrUpdateValue(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, isVisible);
        }

        public static bool IsCurrencySymbolOnStartForCurrency(string currencyCode)
        {
            return AppSettings.GetValueOrDefault(EXCHANGE_RATES_CURRENCY_SYMBOL_ON_START + currencyCode, false);
        }

        public static void SetCurrencySymbolOnStartForCurrency(string currencyCode, bool isOnStart)
        {
            AppSettings.AddOrUpdateValue(EXCHANGE_RATES_CURRENCY_SYMBOL_ON_START + currencyCode, isOnStart);
        }

        public static string GetCustomCurrencySymbol(string currencyCode)
        {
            return AppSettings.GetValueOrDefault(EXCHANGE_RATES_CUSTOM_SYMBOL + currencyCode, null);
        }

        public static void SetCustomCurrencySymbol(string currencyCode, string customSymbol)
        {
            AppSettings.AddOrUpdateValue(EXCHANGE_RATES_CUSTOM_SYMBOL + currencyCode, customSymbol);
        }

        public static string GetLastViewedCurrency()
        {
            return AppSettings.GetValueOrDefault(LAST_VIEWED_EXCHANGE_RATE, DEFAULT_EXCHANGE_RATE);
        }

        public static void SetLastViewedCurrency(string currencyCode)
        {
            AppSettings.AddOrUpdateValue(LAST_VIEWED_EXCHANGE_RATE, currencyCode);
        }

        public static string GetLastBitcoinExchangeRates()
        {
            return AppSettings.GetValueOrDefault(LAST_BITCOIN_EXCHANGE_RATES, null);
        }

        public static void SetLastBitcoinExchangeRates(string rawBitcoinExchangeRates)
        {
            AppSettings.AddOrUpdateValue(LAST_BITCOIN_EXCHANGE_RATES, rawBitcoinExchangeRates);
        }

        public static void SetLastExchangeRatesByUSD(string rawExchangeRatesByUSD)
        {
            AppSettings.AddOrUpdateValue(LAST_EXCHANGE_RATES_BY_USD, rawExchangeRatesByUSD);
        }

        public static string GetLastExchangeRatesByUSD()
        {
            return AppSettings.GetValueOrDefault(LAST_EXCHANGE_RATES_BY_USD, null);
        }

        public static void SetLastYesterdayUSDRate(string yesterdayUSDRateRawData)
        {
            AppSettings.AddOrUpdateValue(LAST_YESTERDAY_USD_RATE, yesterdayUSDRateRawData);
        }

        public static string GetLastYesterdayUSDRate()
        {
            return AppSettings.GetValueOrDefault(LAST_YESTERDAY_USD_RATE, null);
        }

        public static void SetLastSpotUSDRate(string spotUSDRateRawData)
        {
            AppSettings.AddOrUpdateValue(LAST_SPOT_USD_RATE, spotUSDRateRawData);
        }

        public static string GetLastSpotUSDRate()
        {
            return AppSettings.GetValueOrDefault(LAST_SPOT_USD_RATE, null);
        }
    }
}