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

            var liveTileService = Substitute.For<ILiveTileVisibilityService>();

            var bitcoinService = new BitcoinDataService(dataService, liveTileService);
            var result = bitcoinService.GetExchangeRatesAsync().Result;

            Assert.AreEqual(0, result.ExchangeRateList.Count);
        }
    }
}
