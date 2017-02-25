﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    public class ExchangeRates
    {
        public Dictionary<string, BitcoinExchangeRate> ExchangeRateList { get; private set; }

        public DateTime Generated { get; private set; }

        public ExchangeRates()
        {
            Generated = DateTime.Now;
            ExchangeRateList = new Dictionary<string, BitcoinExchangeRate>();
        }

        public ExchangeRates(Dictionary<string, BitcoinExchangeRate> exchangeRateList)
        {
            Generated = DateTime.Now;
            ExchangeRateList = exchangeRateList;
        }
    }
}
