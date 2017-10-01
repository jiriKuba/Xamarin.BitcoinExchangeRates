using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ExchangeRate
    {
        //{ "base": "USD", "date": "2017-02-21", "rates": { 
        //"AUD": 1.3061, "BGN": 1.8561, "BRL": 3.096, "CAD": 1.3135, "CHF": 1.0097, "CNY": 6.8848, "CZK": 25.644, "DKK": 7.0545, "GBP": 0.80535, "HKD": 7.7615, "HRK": 7.0722, "HUF": 291.26, "IDR": 13373, "ILS": 3.7023, "INR": 66.936, "JPY": 113.67, "KRW": 1147.1, "MXN": 20.431, "MYR": 4.4579, "NOK": 8.3646, "NZD": 1.4005, "PHP": 50.289, "PLN": 4.0869, "RON": 4.285, "RUB": 57.738, "SEK": 8.9864, "SGD": 1.4225, "THB": 35.035, "TRY": 3.6266, "ZAR": 13.16, "EUR": 0.94904 
        //} }

        [JsonProperty(PropertyName = "base")]
        public string Base { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
    }
}
