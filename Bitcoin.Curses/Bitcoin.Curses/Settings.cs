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

        public static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static Boolean IsLiveTileVisibility(String currencyCode)
        {
            return AppSettings.GetValueOrDefault<Boolean>(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, false);
        }

        public static void SetLiveTileVisibility(String currencyCode, Boolean isVisible)
        {
            AppSettings.AddOrUpdateValue<Boolean>(EXCHANGE_RATES_LIVE_TITLE_VISIBILITY + currencyCode, isVisible);
        }
    }
}
