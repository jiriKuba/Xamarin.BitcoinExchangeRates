using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    public class ExchangeRate
    {
        //json format: "USD" : {"15m" : 478.68, "last" : 478.68, "buy" : 478.55, "sell" : 478.68,  "symbol" : "$"},

        [JsonProperty(PropertyName = "15m")]
        public Decimal DelayedMarketPrice { get; set; }

        [JsonProperty(PropertyName = "last")]
        public Decimal RecentMarketPrice { get; set; }

        [JsonProperty(PropertyName = "buy")]
        public Decimal Buy { get; set; }

        [JsonProperty(PropertyName = "sell")]
        public Decimal Sell { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public String CurrencySymbol { get; set; }

        public ExchangeRate()
        {

        }
    }
}
