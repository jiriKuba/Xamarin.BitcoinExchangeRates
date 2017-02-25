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
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            // Download the feed.
            var rates = await LoadExchangeRates();

            // Update the live tile with the feed items.
            UpdateTile(rates);

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static async Task<Curses.Models.ExchangeRates> LoadExchangeRates()
        {
            try
            {
                var bitcoinService = new BitcoinDataService(new DataProvideService(), new LiveTileVisibilityService());
                return await bitcoinService.GetExchangeRatesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return null;
        }

        private static void UpdateTile(Curses.Models.ExchangeRates rates)
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
                var tileXml = GetTileXmlTemplate(item.Key, item.Value.RecentMarketPrice.ToString("N2") + item.Value.CurrencySymbol);

                // Create a new tile notification.
                updater.Update(new TileNotification(tileXml));
            }
        }

        /// <summary>
        /// Source: https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/06/30/adaptive-tile-templates-schema-and-documentation/
        /// </summary>
        private static XmlDocument GetTileXmlTemplate(string title, string body)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<tile>");
            sb.AppendLine("<visual>");

            sb.AppendLine("<binding template = \"TileMedium\">");
            sb.AppendLine(String.Format("<text hint-style = \"captionSubtle\">{0}</text>", title));
            sb.AppendLine("<text hint-style = \"caption\" hint-align=\"center\"></text>");//margin
            sb.AppendLine(String.Format("<text hint-style = \"title\" hint-align=\"center\">{0}</text>", body));
            sb.AppendLine("</binding >");

            sb.AppendLine("<binding template = \"TileWide\">");
            sb.AppendLine(String.Format("<text hint-style = \"captionSubtle\">{0}</text>", title));
            sb.AppendLine(String.Format("<text hint-style = \"header\" hint-align=\"center\">{0}</text>", body));
            sb.AppendLine("</binding>");

            sb.AppendLine("<binding template = \"TileLarge\">");
            sb.AppendLine(String.Format("<text hint-style = \"captionSubtle\">{0}</text>", title));
            sb.AppendLine(String.Format("<text hint-style = \"header\" hint-align=\"center\">{0}</text>", body));
            sb.AppendLine("</binding>");

            sb.AppendLine("</visual>");
            sb.AppendLine("</tile>");

            var result = new XmlDocument();
            result.LoadXml(sb.ToString());
            return result;
        }
    }
}