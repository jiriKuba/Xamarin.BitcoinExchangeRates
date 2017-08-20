using Bitcoin.Curses.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Bitcoin.Curses.Services.Interfaces;

namespace Bitcoin.Curses.Tests.Services
{
    [TestClass]
    public class BitcoinDataServiceTest
    {
        [TestMethod]
        public void GetExchangeRatesAsync_NoDataExcpectedWhenJsonAreEmptyNegativeTest()
        {
            var dataService = Substitute.For<IDataProvideService>();
            dataService.GetBitcoinJSONData().Returns(Task.FromResult<string>(null));
            dataService.GetExchangeJSONData().Returns(Task.FromResult<string>(null));
            dataService.GetHistoryJSONData().Returns(Task.FromResult<string>(null));
            dataService.GetSpotBitcoinJSONDataFromUSD().Returns(Task.FromResult<string>(null));

            var rateSettingsApplyService = Substitute.For<IRateSettingsApplyService>();
            var customCurrencySymbolService = Substitute.For<ICustomCurrencySymbolService>();
            var networkService = Substitute.For<INetworkService>();
            networkService.IsInternetAvailable().Returns(Task.FromResult<bool>(true));

            var bitcoinService = new BitcoinDataService(dataService, rateSettingsApplyService, customCurrencySymbolService, networkService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;

            Assert.AreEqual(0, result.ExchangeRateList.Count);
        }

        [TestMethod]
        public void GetExchangeRatesAsync_ExpectedThreeExchangeRatesWhenExchangeAPIReturnsThreeValues()
        {
            var dataService = Substitute.For<IDataProvideService>();
            dataService.GetBitcoinJSONData().Returns(Task.FromResult<string>("{ \"USD\" : {\"15m\" : 1151.57, \"last\" : 1151.57, \"buy\" : 1150.01, \"sell\" : 1151.57, \"symbol\" : \"$\"}, \"EUR\" : {\"15m\" : 1090.41, \"last\" : 1090.41, \"buy\" : 1088.93, \"sell\" : 1090.41, \"symbol\" : \"€\"} }"));
            dataService.GetExchangeJSONData().Returns(Task.FromResult<string>("{\"base\":\"USD\",\"date\":\"2017-02-24\",\"rates\":{\"CZK\":25.47,\"EUR\":0.85179}}"));
            dataService.GetHistoryJSONData().Returns(Task.FromResult<string>("{\"bpi\":{\"2017-05-13\":1759.9613},\"disclaimer\":\"This data was produced from the CoinDesk Bitcoin Price Index. BPI value data returned as USD.\",\"time\":{\"updated\":\"May 14, 2017 00:03:00 UTC\",\"updatedISO\":\"2017-05-14T00:03:00+00:00\"}}"));
            dataService.GetSpotBitcoinJSONDataFromUSD().Returns(Task.FromResult<string>("{\"time\":{\"updated\":\"Aug 3, 2017 16:24:00 UTC\",\"updatedISO\":\"2017-08-03T16:24:00+00:00\",\"updateduk\":\"Aug 3, 2017 at 17:24 BST\"},\"disclaimer\":\"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org\",\"bpi\":{\"USD\":{\"code\":\"USD\",\"rate\":\"2,783.8138\",\"description\":\"United States Dollar\",\"rate_float\":2783.8138}}}"));

            var rateSettingsApplyService = Substitute.For<IRateSettingsApplyService>();
            var customCurrencySymbolService = Substitute.For<ICustomCurrencySymbolService>();
            var networkService = Substitute.For<INetworkService>();
            networkService.IsInternetAvailable().Returns(Task.FromResult<bool>(true));

            var bitcoinService = new BitcoinDataService(dataService, rateSettingsApplyService, customCurrencySymbolService, networkService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;

            //count test
            Assert.AreEqual(3, result.ExchangeRateList.Count);
        }

        [TestMethod]
        public void GetExchangeRatesAsync_NoDataExpectedWhenInternetNotAvailable()
        {
            var dataService = Substitute.For<IDataProvideService>();
            dataService.GetBitcoinJSONData().Returns(Task.FromResult<string>("{ \"USD\" : {\"15m\" : 1151.57, \"last\" : 1151.57, \"buy\" : 1150.01, \"sell\" : 1151.57, \"symbol\" : \"$\"}, \"EUR\" : {\"15m\" : 1090.41, \"last\" : 1090.41, \"buy\" : 1088.93, \"sell\" : 1090.41, \"symbol\" : \"€\"} }"));
            dataService.GetExchangeJSONData().Returns(Task.FromResult<string>("{\"base\":\"USD\",\"date\":\"2017-02-24\",\"rates\":{\"CZK\":25.47,\"EUR\":0.85179}}"));
            dataService.GetHistoryJSONData().Returns(Task.FromResult<string>("{\"bpi\":{\"2017-05-13\":1759.9613},\"disclaimer\":\"This data was produced from the CoinDesk Bitcoin Price Index. BPI value data returned as USD.\",\"time\":{\"updated\":\"May 14, 2017 00:03:00 UTC\",\"updatedISO\":\"2017-05-14T00:03:00+00:00\"}}"));
            dataService.GetSpotBitcoinJSONDataFromUSD().Returns(Task.FromResult<string>("{\"time\":{\"updated\":\"Aug 3, 2017 16:24:00 UTC\",\"updatedISO\":\"2017-08-03T16:24:00+00:00\",\"updateduk\":\"Aug 3, 2017 at 17:24 BST\"},\"disclaimer\":\"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org\",\"bpi\":{\"USD\":{\"code\":\"USD\",\"rate\":\"2,783.8138\",\"description\":\"United States Dollar\",\"rate_float\":2783.8138}}}"));

            var rateSettingsApplyService = Substitute.For<IRateSettingsApplyService>();
            var customCurrencySymbolService = Substitute.For<ICustomCurrencySymbolService>();
            var networkService = Substitute.For<INetworkService>();
            networkService.IsInternetAvailable().Returns(Task.FromResult<bool>(false));

            var bitcoinService = new BitcoinDataService(dataService, rateSettingsApplyService, customCurrencySymbolService, networkService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;
            Assert.AreEqual(0, result.ExchangeRateList.Count);
        }

        [TestMethod]
        public void GetExchangeRatesAsync_ExpectedTwoDecimalRoundByAPIReturnsValues()
        {
            var dataService = Substitute.For<IDataProvideService>();
            dataService.GetBitcoinJSONData().Returns(Task.FromResult<string>("{ \"USD\" : {\"15m\" : 1151.57, \"last\" : 1151.57, \"buy\" : 1150.01, \"sell\" : 1151.57, \"symbol\" : \"$\"}, \"EUR\" : {\"15m\" : 1090.41, \"last\" : 1090.41, \"buy\" : 1088.93, \"sell\" : 1090.41, \"symbol\" : \"€\"} }"));
            dataService.GetExchangeJSONData().Returns(Task.FromResult<string>("{\"base\":\"USD\",\"date\":\"2017-02-24\",\"rates\":{\"CZK\":25.47,\"EUR\":0.85179}}"));
            dataService.GetHistoryJSONData().Returns(Task.FromResult<string>("{\"bpi\":{\"2017-05-13\":1759.9613},\"disclaimer\":\"This data was produced from the CoinDesk Bitcoin Price Index. BPI value data returned as USD.\",\"time\":{\"updated\":\"May 14, 2017 00:03:00 UTC\",\"updatedISO\":\"2017-05-14T00:03:00+00:00\"}}"));
            dataService.GetSpotBitcoinJSONDataFromUSD().Returns(Task.FromResult<string>("{\"time\":{\"updated\":\"Aug 3, 2017 16:24:00 UTC\",\"updatedISO\":\"2017-08-03T16:24:00+00:00\",\"updateduk\":\"Aug 3, 2017 at 17:24 BST\"},\"disclaimer\":\"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org\",\"bpi\":{\"USD\":{\"code\":\"USD\",\"rate\":\"2,783.8138\",\"description\":\"United States Dollar\",\"rate_float\":2783.8138}}}"));

            var rateSettingsApplyService = Substitute.For<IRateSettingsApplyService>();
            var customCurrencySymbolService = Substitute.For<ICustomCurrencySymbolService>();
            var networkService = Substitute.For<INetworkService>();
            networkService.IsInternetAvailable().Returns(Task.FromResult<bool>(true));

            var bitcoinService = new BitcoinDataService(dataService, rateSettingsApplyService, customCurrencySymbolService, networkService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;

            //property test
            Assert.AreEqual(1150.01M, result.ExchangeRateList["USD"].Buy);
            Assert.AreEqual(1151.57M, result.ExchangeRateList["USD"].Sell);
            Assert.AreEqual("$", result.ExchangeRateList["USD"].CurrencySymbol);
            Assert.AreEqual(2783.81M, result.ExchangeRateList["USD"].DelayedMarketPrice);
            Assert.AreEqual(2783.81M, result.ExchangeRateList["USD"].RecentMarketPrice);
            Assert.AreEqual(1759.96M, result.ExchangeRateList["USD"].YesterdayRate);

            Assert.AreEqual(979.57M, result.ExchangeRateList["EUR"].Buy);
            Assert.AreEqual(980.90M, result.ExchangeRateList["EUR"].Sell);
            Assert.AreEqual("€", result.ExchangeRateList["EUR"].CurrencySymbol);
            Assert.AreEqual(2371.22M, result.ExchangeRateList["EUR"].DelayedMarketPrice);
            Assert.AreEqual(2371.22M, result.ExchangeRateList["EUR"].RecentMarketPrice);
            Assert.AreEqual(1499.12M, result.ExchangeRateList["EUR"].YesterdayRate);

            Assert.AreEqual(29290.75M, result.ExchangeRateList["CZK"].Buy);
            Assert.AreEqual(29330.49M, result.ExchangeRateList["CZK"].Sell);
            Assert.AreEqual("Kč", result.ExchangeRateList["CZK"].CurrencySymbol);
            Assert.AreEqual(70903.64M, result.ExchangeRateList["CZK"].DelayedMarketPrice);
            Assert.AreEqual(70903.64M, result.ExchangeRateList["CZK"].RecentMarketPrice);
            Assert.AreEqual(44826.21M, result.ExchangeRateList["CZK"].YesterdayRate);
        }
    }
}