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
        private readonly IRateSettingsApplyService _rateSettingsApplyService;
        private readonly ICustomCurrencySymbolService _customCurrencyCodeService;
        private readonly INetworkService _networkService;

        public BitcoinDataService(IDataProvideService dataProvideService, IRateSettingsApplyService rateSettingsApplyService,
            ICustomCurrencySymbolService customCurrencyCodeService, INetworkService networkService)
        {
            _helper = new CurrencyHelper();
            _dataProvideService = dataProvideService;
            _rateSettingsApplyService = rateSettingsApplyService;
            _customCurrencyCodeService = customCurrencyCodeService;
            _networkService = networkService;
        }

        public async Task<ExchangeRates> GetExchangeRatesAsync()
        {
            try
            {
                var isInternetAvailable = await _networkService.IsInternetAvailable();
                if (!isInternetAvailable)
                {
                    throw new Exception("Internet connection is not available!");
                }
                else
                {
                    var bitcoinRateValues = await GetBitcoinRateValues();
                    var exchangeRatesByUSD = await GetExchangeRatesByUSD();
                    var yesterdayUSDRate = await GetYesterdayUSDRate();
                    var spotUSDRate = await GetSpotUSDRate();

                    bitcoinRateValues = GetUSDPriceOnly(bitcoinRateValues);

                    AddSpotBTCPrice(bitcoinRateValues, spotUSDRate);

                    AddAlternativeRatesToBitcoinRateList(bitcoinRateValues, exchangeRatesByUSD);

                    AddYesterdayRatesToBitcoinRateList(bitcoinRateValues, exchangeRatesByUSD, yesterdayUSDRate);

                    _rateSettingsApplyService.ApplySettingsToModels(bitcoinRateValues);
                    _customCurrencyCodeService.AddCustomCurrencySymbolToModel(bitcoinRateValues);
                    var result = new ExchangeRates(bitcoinRateValues
                        .OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.Value));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ExceptionMessage>(new ExceptionMessage(ex));
                return new ExchangeRates();
            }
        }

        private IDictionary<string, BitcoinExchangeRate> GetUSDPriceOnly(IDictionary<string, BitcoinExchangeRate> bitcoinRateValues)
        {
            return bitcoinRateValues.Where(x => x.Key == USD_RATE_KEY)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private void AddSpotBTCPrice(IDictionary<string, BitcoinExchangeRate> bitcoinRateValues, SpotUSDExchangeRate spotUSDRate)
        {
            bitcoinRateValues[USD_RATE_KEY].RecentMarketPrice = Math.Round(spotUSDRate.ExchangeRateValue, 2);
            bitcoinRateValues[USD_RATE_KEY].DelayedMarketPrice = Math.Round(bitcoinRateValues[USD_RATE_KEY].RecentMarketPrice, 2);
        }

        private async Task<IDictionary<string, BitcoinExchangeRate>> GetBitcoinRateValues()
        {
            try
            {
                var rawBitcoinExchangeRates = await _dataProvideService.GetBitcoinJSONData();
                var bitcoinRateValues = JsonConvert.DeserializeObject<Dictionary<string, BitcoinExchangeRate>>(rawBitcoinExchangeRates);

                _rateSettingsApplyService.SetLastBitcoinExchangeRates(rawBitcoinExchangeRates);

                return bitcoinRateValues;
            }
            catch
            {
                var historyValue = _rateSettingsApplyService.GetLastBitcoinExchangeRates();
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
                var rawExchangeRatesByUSD = await _dataProvideService.GetExchangeJSONData();
                var exchangeRatesByUSD = JsonConvert.DeserializeObject<ExchangeRate>(rawExchangeRatesByUSD);

                _rateSettingsApplyService.SetLastExchangeRatesByUSD(rawExchangeRatesByUSD);

                return exchangeRatesByUSD;
            }
            catch
            {
                var historyValue = _rateSettingsApplyService.GetLastExchangeRatesByUSD();
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

                _rateSettingsApplyService.SetLastYesterdayUSDRate(yesterdayUSDRateRawData);

                return yesterdayUSDRate;
            }
            catch
            {
                var historyValue = _rateSettingsApplyService.GetLastYesterdayUSDRate();
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

        private async Task<SpotUSDExchangeRate> GetSpotUSDRate()
        {
            try
            {
                var spotUSDRateRawData = await _dataProvideService.GetSpotBitcoinJSONDataFromUSD();
                var spotUSDRate = JsonConvert.DeserializeObject<SpotUSDExchangeRate>(spotUSDRateRawData);

                _rateSettingsApplyService.SetLastSpotUSDRate(spotUSDRateRawData);

                return spotUSDRate;
            }
            catch
            {
                var lastValueData = _rateSettingsApplyService.GetLastSpotUSDRate();
                if (string.IsNullOrEmpty(lastValueData))
                {
                    throw new Exception("Unable to load bitcoin exchange rate history. Try again later.");
                }
                else
                {
                    return JsonConvert.DeserializeObject<SpotUSDExchangeRate>(lastValueData);
                }
            }
        }

        private void AddYesterdayRatesToBitcoinRateList(IDictionary<string, BitcoinExchangeRate> bitcoinRates, ExchangeRate alternativeRates, ExchangeRateHistory yesterdayUSDRate)
        {
            if (yesterdayUSDRate != null && bitcoinRates != null && bitcoinRates.ContainsKey(USD_RATE_KEY))
            {
                if (alternativeRates != null && alternativeRates.Rates != null && alternativeRates.Rates.Count > 0)
                {
                    foreach (var rate in alternativeRates.Rates)
                    {
                        if (bitcoinRates.ContainsKey(rate.Key))
                        {
                            bitcoinRates[rate.Key].YesterdayRate = Math.Round(rate.Value * yesterdayUSDRate.HistoryValue, 2);
                        }
                    }
                }

                if (bitcoinRates.ContainsKey(USD_RATE_KEY))
                {
                    bitcoinRates[USD_RATE_KEY].YesterdayRate = Math.Round(yesterdayUSDRate.HistoryValue, 2);
                }
            }
        }

        public void AddAlternativeRatesToBitcoinRateList(IDictionary<string, BitcoinExchangeRate> bitcoinRates, ExchangeRate alternativeRates)
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
                            bitcoinRates.Add(rate.Key, new BitcoinExchangeRate
                            {
                                Buy = Math.Round(usdBitcoinRate.Buy * rate.Value, 2),
                                CurrencySymbol = _helper.CurrencySymbols.ContainsKey(rate.Key) ? _helper.CurrencySymbols[rate.Key] : null,
                                DelayedMarketPrice = Math.Round(usdBitcoinRate.DelayedMarketPrice * rate.Value, 2),
                                RecentMarketPrice = Math.Round(usdBitcoinRate.RecentMarketPrice * rate.Value, 2),
                                Sell = Math.Round(usdBitcoinRate.Sell * rate.Value, 2),
                            });
                        }
                    }
                }
            }
        }
    }
}