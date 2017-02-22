using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    public class ExchangeRates
    {
        public Dictionary<String, BitcoinExchangeRate> ExchangeRateList { get; private set; }

        public DateTime Generated { get; private set; }

        public ExchangeRates()
        {
            this.Generated = DateTime.Now;
            this.ExchangeRateList = new Dictionary<String, BitcoinExchangeRate>();
        }

        public ExchangeRates(Dictionary<String, BitcoinExchangeRate> exchangeRateList)
        {
            this.Generated = DateTime.Now;
            this.ExchangeRateList = exchangeRateList;
        }
    }
}
