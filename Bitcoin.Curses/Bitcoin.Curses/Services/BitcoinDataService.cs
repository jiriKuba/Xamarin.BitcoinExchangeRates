using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitcoin.Curses.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Bitcoin.Curses.Services
{
    public class BitcoinDataService : IBitcoinDataService
    {
        private const String EXCHANGE_RATES_API_URL = "https://blockchain.info/ticker";

        public BitcoinDataService()
        {

        }

        public async Task<ExchangeRates> GetExchangeRatesAsync()
        {
            try
            {
                String rawExchangeRates = await this.GetStringFromURLAsync(EXCHANGE_RATES_API_URL);
                Dictionary<String, ExchangeRate> values = JsonConvert.DeserializeObject<Dictionary<String, ExchangeRate>>(rawExchangeRates);
                ExchangeRates result = new ExchangeRates(values);
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

        private async Task<String> GetStringFromURLAsync(String url)
        {
            String rawJson = null;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                rawJson = await response.Content.ReadAsStringAsync();
            }

            return rawJson;
        }
    }
}
