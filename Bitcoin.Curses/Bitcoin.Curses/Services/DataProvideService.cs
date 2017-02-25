using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services
{
    public class DataProvideService : IDataProvideService
    {
        //http://www.coindesk.com/api/ maybe better api
        private const string BITCOIN_EXCHANGE_RATES_API_URL = "https://blockchain.info/ticker";
        private const string EXCHANGE_RATES_API_URL = "http://api.fixer.io/latest?base=USD";

        public async Task<String> GetBitcoinJSONData()
        {
            return await GetStringFromURLAsync(BITCOIN_EXCHANGE_RATES_API_URL);
        }

        public async Task<String> GetExchangeJSONData()
        {
            return await GetStringFromURLAsync(EXCHANGE_RATES_API_URL);
        }

        public async Task<String> GetStringFromURLAsync(string url)
        {
            string rawJson = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                rawJson = await response.Content.ReadAsStringAsync();
            }

            return rawJson;
        }
    }
}
