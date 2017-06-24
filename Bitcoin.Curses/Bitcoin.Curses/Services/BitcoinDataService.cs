using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitcoin.Curses.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Bitcoin.Curses.Helpers;
using GalaSoft.MvvmLight.Messaging;
using Bitcoin.Curses.Messages;

namespace Bitcoin.Curses.Services
{
    public class BitcoinDataService : IBitcoinDataService
    {
        private const string USD_RATE_KEY = "USD";

        private readonly CurrencyHelper _helper;
        private readonly IDataProvideService _dataProvideService;
        private readonly ILiveTileVisibilityService _liveTileVisibilityService;
        private readonly ICustomCurrencySymbolServise _customCurrencyCodeServise;

        public BitcoinDataService(IDataProvideService dataProvideService, ILiveTileVisibilityService liveTileVisibilityService,
            ICustomCurrencySymbolServise customCurrencyCodeServise)
        {
            _helper = new CurrencyHelper();
            _dataProvideService = dataProvideService;
            _liveTileVisibilityService = liveTileVisibilityService;
            _customCurrencyCodeServise = customCurrencyCodeServise;
        }

        public async Task<ExchangeRates> GetExchangeRatesAsync()
        {
            try
            {
                var bitcoinRateValues = await GetBitcoinRateValues();
                var exchangeRatesByUSD = await GetExchangeRatesByUSD();
                var yesterdayUSDRate = await GetYesterdayUSDRate();

                AddAlternativeRatesToBitcoinRateList(bitcoinRateValues, exchangeRatesByUSD);

                AddYesterdayRatesToBitcoinRateList(bitcoinRateValues, exchangeRatesByUSD, yesterdayUSDRate);

                _liveTileVisibilityService.AddLiveTileVisibilityToModel(bitcoinRateValues);
                _customCurrencyCodeServise.AddCustomCurrencySymbolToModel(bitcoinRateValues);
                var result = new ExchangeRates(bitcoinRateValues
                    .OrderBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Value));
                return result;

                //test data
                //await Task.Delay(TimeSpan.FromSeconds(3));

                //ExchangeRates mock = new ExchangeRates();
                //mock.ExchangeRateList.Add("USD", new ExchangeRate() { DelayedMarketPrice = 478.68M, RecentMarketPrice = 478.68M, Buy = 478.55M, Sell = 478.68M, CurrencySymbol = "$" });
                //mock.ExchangeRateList.Add("JPY", new ExchangeRate() { DelayedMarketPrice = 51033.99M, RecentMarketPrice = 51033.99M, Buy = 51020.13M, Sell = 51033.99M, CurrencySymbol = "¥" });
                //return mock;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ExceptionMessage>(new ExceptionMessage(ex));
                return new ExchangeRates();
            }
        }

        private async Task<Dictionary<string, BitcoinExchangeRate>> GetBitcoinRateValues()
        {
            try
            {
                var rawBitcoinExchangeRates = await _dataProvideService.GetBitcoinJSONData();
                var bitcoinRateValues = JsonConvert.DeserializeObject<Dictionary<string, BitcoinExchangeRate>>(rawBitcoinExchangeRates);

                Settings.SetLastBitcoinExchangeRates(rawBitcoinExchangeRates);

                return bitcoinRateValues;
            }
            catch
            {
                var historyValue = Settings.GetLastBitcoinExchangeRates();
                if (string.IsNullOrEmpty(historyValue))
                {
                    throw new Exception("Unable to load bitcoin exchange rates data. Try again later.");
                }
                else
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, BitcoinExchangeRate>>(historyValue);
                }
            }
        }

        private async Task<ExchangeRate> GetExchangeRatesByUSD()
        {
            try
            {
                throw new ArgumentException();
                var rawExchangeRatesByUSD = await _dataProvideService.GetExchangeJSONData();
                var exchangeRatesByUSD = JsonConvert.DeserializeObject<ExchangeRate>(rawExchangeRatesByUSD);

                Settings.SetLastExchangeRatesByUSD(rawExchangeRatesByUSD);

                return exchangeRatesByUSD;
            }
            catch
            {
                var historyValue = Settings.GetLastExchangeRatesByUSD();
                if (string.IsNullOrEmpty(historyValue))
                {
                    throw new Exception("Unable to load USD exchange rates data. Try again later.");
                }
                else
                {
                    return JsonConvert.DeserializeObject<ExchangeRate>(historyValue);
                }
            }
        }

        private async Task<ExchangeRateHistory> GetYesterdayUSDRate()
        {
            try
            {
                var yesterdayUSDRateRawData = await _dataProvideService.GetHistoryJSONData();
                var yesterdayUSDRate = JsonConvert.DeserializeObject<ExchangeRateHistory>(yesterdayUSDRateRawData);

                Settings.SetLastYesterdayUSDRate(yesterdayUSDRateRawData);

                return yesterdayUSDRate;
            }
            catch
            {
                var historyValue = Settings.GetLastYesterdayUSDRate();
                if (string.IsNullOrEmpty(historyValue))
                {
                    throw new Exception("Unable to load bitcoin exchange rate history. Try again later.");
                }
                else
                {
                    return JsonConvert.DeserializeObject<ExchangeRateHistory>(historyValue);
                }
            }
        }

        private void AddYesterdayRatesToBitcoinRateList(Dictionary<string, BitcoinExchangeRate> bitcoinRates, ExchangeRate alternativeRates, ExchangeRateHistory yesterdayUSDRate)
        {
            if (yesterdayUSDRate != null && bitcoinRates != null && bitcoinRates.ContainsKey(USD_RATE_KEY))
            {
                if (alternativeRates != null && alternativeRates.Rates != null && alternativeRates.Rates.Count > 0)
                {
                    foreach (var rate in alternativeRates.Rates)
                    {
                        if (bitcoinRates.ContainsKey(rate.Key))
                        {
                            bitcoinRates[rate.Key].YesterdayRate = rate.Value * yesterdayUSDRate.HistoryValue;
                        }
                    }
                }

                if (bitcoinRates.ContainsKey(USD_RATE_KEY))
                {
                    bitcoinRates[USD_RATE_KEY].YesterdayRate = yesterdayUSDRate.HistoryValue;
                }
            }
        }

        public void AddAlternativeRatesToBitcoinRateList(Dictionary<string, BitcoinExchangeRate> bitcoinRates, ExchangeRate alternativeRates)
        {
            if (bitcoinRates != null && bitcoinRates.ContainsKey(USD_RATE_KEY))
            {
                var usdBitcoinRate = bitcoinRates[USD_RATE_KEY];
                if (alternativeRates != null && alternativeRates.Rates != null && alternativeRates.Rates.Count > 0)
                {
                    foreach (var rate in alternativeRates.Rates)
                    {
                        if (!bitcoinRates.ContainsKey(rate.Key))
                        {
                            bitcoinRates.Add(rate.Key, new BitcoinExchangeRate()
                            {
                                Buy = usdBitcoinRate.Buy * rate.Value,
                                CurrencySymbol = _helper.CurrencySymbols.ContainsKey(rate.Key) ? _helper.CurrencySymbols[rate.Key] : null,
                                DelayedMarketPrice = usdBitcoinRate.DelayedMarketPrice * rate.Value,
                                RecentMarketPrice = usdBitcoinRate.RecentMarketPrice * rate.Value,
                                Sell = usdBitcoinRate.Sell * rate.Value,
                            });
                        }
                    }
                }
            }
        }
    }
}