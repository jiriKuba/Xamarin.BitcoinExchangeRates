using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    public class BitcoinExchangeRate
    {
        //json format: "USD" : {"15m" : 478.68, "last" : 478.68, "buy" : 478.55, "sell" : 478.68,  "symbol" : "$"},

        [JsonProperty(PropertyName = "15m")]
        public decimal DelayedMarketPrice { get; set; }

        [JsonProperty(PropertyName = "last")]
        public decimal RecentMarketPrice { get; set; }

        [JsonProperty(PropertyName = "buy")]
        public decimal Buy { get; set; }

        [JsonProperty(PropertyName = "sell")]
        public decimal Sell { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string CurrencySymbol { get; set; }

        [JsonIgnore]
        public bool IsVisibleOnLiveTile { get; set; }

        [JsonIgnore]
        public string CustomCurrencySymbol { get; set; }

        [JsonIgnore]
        public decimal YesterdayRate { get; set; }

        [JsonIgnore]
        public bool IsCurrencySymbolOnStart { get; internal set; }

        public BitcoinExchangeRate()
        {
            IsVisibleOnLiveTile = false;
            IsCurrencySymbolOnStart = false;
        }
    }
}