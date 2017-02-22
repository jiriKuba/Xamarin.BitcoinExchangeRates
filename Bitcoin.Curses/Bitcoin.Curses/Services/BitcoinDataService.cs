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

namespace Bitcoin.Curses.Services
{
    public class BitcoinDataService : IBitcoinDataService
    {
        //http://www.coindesk.com/api/ maybe better api
        private const string BITCOIN_EXCHANGE_RATES_API_URL = "https://blockchain.info/ticker";
        private const string EXCHANGE_RATES_API_URL = "http://api.fixer.io/latest?base=USD";
        private const string USD_RATE_KEY = "USD";

        private readonly CurrencyHelper _helper;

        public BitcoinDataService()
        {
            _helper = new CurrencyHelper();
        }

        public async Task<ExchangeRates> GetExchangeRatesAsync()
        {
            try
            {
                var rawBitcoinExchangeRates = await GetStringFromURLAsync(BITCOIN_EXCHANGE_RATES_API_URL);
                var bitcoinRateValues = JsonConvert.DeserializeObject<Dictionary<string, BitcoinExchangeRate>>(rawBitcoinExchangeRates);

                var rawExchangeRatesByUSD = await GetStringFromURLAsync(EXCHANGE_RATES_API_URL);
                var exchangeRatesByUSD = JsonConvert.DeserializeObject<ExchangeRate>(rawExchangeRatesByUSD);

                AddAlternativeRatesToBitcoinRateList(bitcoinRateValues, exchangeRatesByUSD);

                AddLiveTileVisibilityToModel(bitcoinRateValues);
                var result = new ExchangeRates(bitcoinRateValues
                    .OrderBy(x=>x.Key)
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
                App.Instance.DisplayAlert("Alert", ex.Message);
                return new ExchangeRates(); 
            }            
        }

        public void SetExchangeRateVisibleOnLiveTile(string exchangeRateKey, bool isVisible)
        {
            Settings.SetLiveTileVisibility(exchangeRateKey, isVisible);
        }

        private void AddLiveTileVisibilityToModel(Dictionary<string, BitcoinExchangeRate> models)
        {
            if (models != null)
            {
                foreach (var rate in models)
                {
                    rate.Value.IsVisibleOnLiveTile = Settings.IsLiveTileVisibility(rate.Key);
                }
            }
        }

        private async Task<String> GetStringFromURLAsync(string url)
        {
            string rawJson = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                rawJson = await response.Content.ReadAsStringAsync();
            }

            return rawJson;
        }

        private void AddAlternativeRatesToBitcoinRateList(Dictionary<string, BitcoinExchangeRate> bitcoinRates, ExchangeRate alternativeRates)
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
                                IsVisibleOnLiveTile = false, //todo: load
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
