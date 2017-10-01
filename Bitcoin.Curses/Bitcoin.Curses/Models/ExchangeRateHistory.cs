using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ExchangeRateHistory
    {
        //{"bpi":{"2017-05-13":1759.9613},"disclaimer":"This data was produced from the CoinDesk Bitcoin Price Index. BPI value data returned as USD.","time":{"updated":"May 14, 2017 00:03:00 UTC","updatedISO":"2017-05-14T00:03:00+00:00"}}

        [JsonProperty(PropertyName = "bpi")]
        public Dictionary<string, decimal> ExchangeRateHistoryValues { get; set; }

        [JsonIgnore]
        public decimal HistoryValue
        {
            get
            {
                if (ExchangeRateHistoryValues != null && ExchangeRateHistoryValues.Count > 0)
                {
                    return ExchangeRateHistoryValues.First().Value;
                }

                return decimal.Zero;
            }
        }

        [JsonProperty(PropertyName = "disclaimer")]
        public string Disclaimer { get; set; }
    }
}