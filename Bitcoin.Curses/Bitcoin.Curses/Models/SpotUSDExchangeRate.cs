using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SpotUSDExchangeRate
    {
        //{"time":{"updated":"Aug 3, 2017 16:24:00 UTC","updatedISO":"2017-08-03T16:24:00+00:00","updateduk":"Aug 3, 2017 at 17:24 BST"},"disclaimer":"This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org","bpi":{"USD":{"code":"USD","rate":"2,783.8138","description":"United States Dollar","rate_float":2783.8138}}}
        [JsonProperty(PropertyName = "bpi")]
        public Dictionary<string, SpotUSDExchangeRateValue> Values { get; set; }

        [JsonIgnore]
        public decimal ExchangeRateValue
        {
            get
            {
                if (Values != null && Values.Count > 0)
                {
                    return Values.First().Value.Rate;
                }

                return decimal.Zero;
            }
        }
    }
}