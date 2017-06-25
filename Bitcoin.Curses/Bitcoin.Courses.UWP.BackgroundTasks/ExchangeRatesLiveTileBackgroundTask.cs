using Bitcoin.Curses;
using Bitcoin.Curses.Models;
using Bitcoin.Curses.Services;
using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace Bitcoin.Courses.UWP.BackgroundTasks
{
    public sealed class ExchangeRatesLiveTileBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            var deferral = taskInstance.GetDeferral();

            // Download the feed.
            var rates = await LoadExchangeRates();

            // Update the live tile with the feed items.
            UpdateTile(rates);

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static async Task<ExchangeRates> LoadExchangeRates()
        {
            try
            {
                var bitcoinService = new BitcoinDataService(new DataProvideService(), new LiveTileVisibilityService(), new CustomCurrencySymbolServise());
                return await bitcoinService.GetExchangeRatesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return null;
        }

        private static void UpdateTile(ExchangeRates rates)
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();

            if (rates == null || rates.ExchangeRateList == null || rates.ExchangeRateList.Count == 0)
            {
                updater.EnableNotificationQueue(false);
                updater.Clear();
                return;
            }

            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Create a tile notification for each feed item.
            var topFiveRates = rates.ExchangeRateList
                .Where(r => r.Value.IsVisibleOnLiveTile)
                .Take(5);

            foreach (var item in topFiveRates)
            {
                //wide tile
                var currencySymbol = string.IsNullOrEmpty(item.Value.CustomCurrencySymbol) ? item.Value.CurrencySymbol : item.Value.CustomCurrencySymbol;

                var marketPriceDifference = GetMarketPriceDifference(item.Value.YesterdayRate, item.Value.RecentMarketPrice);
                var marketPriceDifferenceLabel = GetMarketPriceDifferenceLabel(marketPriceDifference);

                var tileXml = GetTileXmlTemplate(item.Key, item.Value.RecentMarketPrice.ToString("N2") + currencySymbol, marketPriceDifferenceLabel);

                // Create a new tile notification.
                updater.Update(new TileNotification(tileXml));
            }
        }

        /// <summary>
        /// Source: https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/06/30/adaptive-tile-templates-schema-and-documentation/
        /// </summary>
        private static XmlDocument GetTileXmlTemplate(string title, string body, string marketPriceDifferenceLabel)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<tile>");
            sb.AppendLine($"<visual branding=\"name\" displayName=\"BTC/{title}\">");

            sb.AppendLine($"<binding template = \"TileMedium\" hint-textStacking=\"center\" branding=\"name\" displayName=\"BTC/{title}\">");
            sb.AppendLine("<text hint-style = \"title\" hint-align=\"center\"></text>"); //margin
            sb.AppendLine($"<text hint-style = \"base\" hint-align=\"center\">{body}</text>");
            sb.AppendLine($"<text hint-style = \"captionSubtle\" hint-align=\"center\">{marketPriceDifferenceLabel}</text>");
            sb.AppendLine("</binding >");

            sb.AppendLine($"<binding template = \"TileWide\" hint-textStacking=\"center\" branding=\"name\" displayName=\"BTC/{title}\">");
            sb.AppendLine("<text hint-style = \"subtitle\" hint-align=\"center\"></text>"); //margin
            sb.AppendLine($"<text hint-style = \"titleNumeral\" hint-align=\"center\">{body}</text>");
            sb.AppendLine($"<text hint-style = \"captionSubtle\" hint-align=\"center\">{marketPriceDifferenceLabel}</text>");
            sb.AppendLine("</binding>");

            sb.AppendLine($"<binding template = \"TileLarge\" hint-textStacking=\"center\" branding=\"name\" displayName=\"BTC/{title}\">");
            sb.AppendLine("<text hint-style = \"titleNumeral\" hint-align=\"center\"></text>"); //margin
            sb.AppendLine($"<text hint-style = \"subheader\" hint-align=\"center\">{body}</text>");
            sb.AppendLine($"<text hint-style = \"bodySubtle\" hint-align=\"center\">{marketPriceDifferenceLabel}</text>");
            sb.AppendLine("</binding>");

            sb.AppendLine("</visual>");
            sb.AppendLine("</tile>");

            var result = new XmlDocument();
            result.LoadXml(sb.ToString());
            return result;
        }

        private static string GetMarketPriceDifferenceLabel(decimal marketPriceDifference)
        {
            var rounded = Math.Round(marketPriceDifference, 2);
            if (rounded >= 0)
            {
                return "+" + rounded.ToString("N2") + " ";
            }
            else if (rounded < 0)
            {
                return "-" + Math.Abs(rounded).ToString("N2") + " ";
            }
            else
            {
                return "+" + decimal.Zero.ToString("N2") + " ";
            }
        }

        private static decimal GetMarketPriceDifference(decimal yesterdayRate, decimal recentMarketPrice)
        {
            return yesterdayRate == 0 ? 0 : (recentMarketPrice - yesterdayRate);
        }
    }
}