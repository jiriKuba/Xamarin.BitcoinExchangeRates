using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ExchangeRates
    {
        public IDictionary<string, BitcoinExchangeRate> ExchangeRateList { get; private set; }

        public DateTime Generated { get; private set; }

        public ExchangeRates()
        {
            Generated = DateTime.Now;
            ExchangeRateList = new Dictionary<string, BitcoinExchangeRate>();
        }

        public ExchangeRates(IDictionary<string, BitcoinExchangeRate> exchangeRateList)
        {
            Generated = DateTime.Now;
            ExchangeRateList = exchangeRateList;
        }
    }
}