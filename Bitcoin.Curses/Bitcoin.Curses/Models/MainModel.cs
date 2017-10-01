using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class MainModel
    {
        public ExchangeRates ExchangeRates { get; set; }

        public MainModel()
        {

        }

        public MainModel(ExchangeRates exchangeRates)
            :this()
        {
            ExchangeRates = exchangeRates;
        }
    }
}
