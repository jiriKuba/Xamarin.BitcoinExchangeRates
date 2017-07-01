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
        public void GetExchangeRatesAsyncNegativeTest()
        {
            var dataService = Substitute.For<IDataProvideService>();
            dataService.GetBitcoinJSONData().Returns(Task.FromResult<string>(null));
            dataService.GetExchangeJSONData().Returns(Task.FromResult<string>(null));

            var liveTileService = Substitute.For<IRateSettingsApplyService>();

            var bitcoinService = new BitcoinDataService(dataService, liveTileService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;

            Assert.AreEqual(0, result.ExchangeRateList.Count);
        }

        [TestMethod]
        public void GetExchangeRatesAsyncPositiveTest()
        {
            var dataService = Substitute.For<IDataProvideService>();
            dataService.GetBitcoinJSONData().Returns(Task.FromResult<string>("{ \"USD\" : {\"15m\" : 1151.57, \"last\" : 1151.57, \"buy\" : 1150.01, \"sell\" : 1151.57, \"symbol\" : \"$\"}, \"EUR\" : {\"15m\" : 1090.41, \"last\" : 1090.41, \"buy\" : 1088.93, \"sell\" : 1090.41, \"symbol\" : \"€\"} }"));
            dataService.GetExchangeJSONData().Returns(Task.FromResult<string>("{\"base\":\"USD\",\"date\":\"2017-02-24\",\"rates\":{\"CZK\":25.47}}"));

            var liveTileService = Substitute.For<IRateSettingsApplyService>();

            var bitcoinService = new BitcoinDataService(dataService, liveTileService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;
            var expectedResult = new Models.ExchangeRates();
            //expectedResult.ExchangeRateList 

            //count test
            Assert.AreEqual(3, result.ExchangeRateList.Count);

            //property test
            Assert.AreEqual(1150.01M, result.ExchangeRateList["USD"].Buy);
            Assert.AreEqual(1151.57M, result.ExchangeRateList["USD"].Sell);
            Assert.AreEqual("$", result.ExchangeRateList["USD"].CurrencySymbol);
            Assert.AreEqual(1151.57M, result.ExchangeRateList["USD"].DelayedMarketPrice);
            Assert.AreEqual(1151.57M, result.ExchangeRateList["USD"].RecentMarketPrice);

            Assert.AreEqual(1088.93M, result.ExchangeRateList["EUR"].Buy);
            Assert.AreEqual(1090.41M, result.ExchangeRateList["EUR"].Sell);
            Assert.AreEqual("€", result.ExchangeRateList["EUR"].CurrencySymbol);
            Assert.AreEqual(1090.41M, result.ExchangeRateList["EUR"].DelayedMarketPrice);
            Assert.AreEqual(1090.41M, result.ExchangeRateList["EUR"].RecentMarketPrice);

            Assert.AreEqual(29290.7547M, result.ExchangeRateList["CZK"].Buy);
            Assert.AreEqual(29330.4879M, result.ExchangeRateList["CZK"].Sell);
            Assert.AreEqual("Kč", result.ExchangeRateList["CZK"].CurrencySymbol);
            Assert.AreEqual(29330.4879M, result.ExchangeRateList["CZK"].DelayedMarketPrice);
            Assert.AreEqual(29330.4879M, result.ExchangeRateList["CZK"].RecentMarketPrice);

        }
    }
}
