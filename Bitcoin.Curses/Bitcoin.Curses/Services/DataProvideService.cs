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

        private const string BITCOIN_EXCHANGE_RATES_HISTORY_API_URL = "http://api.coindesk.com/v1/bpi/historical/close.json?for=yesterday";
        private const string EXCHANGE_RATES_API_URL = "http://api.fixer.io/latest?base=USD";

        public async Task<string> GetBitcoinJSONData()
        {
            return await GetStringFromURLAsync(BITCOIN_EXCHANGE_RATES_API_URL);
        }

        public async Task<string> GetExchangeJSONData()
        {
            return await GetStringFromURLAsync(EXCHANGE_RATES_API_URL);
        }

        public async Task<string> GetHistoryJSONData()
        {
            return await GetStringFromURLAsync(BITCOIN_EXCHANGE_RATES_HISTORY_API_URL);
        }

        public async Task<string> GetStringFromURLAsync(string url)
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