using NodaMoney;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Helpers
{
    public class CurrencyHelper
    {
        public readonly IDictionary<string, string> CurrencySymbols;

        public CurrencyHelper()
        {
            CurrencySymbols = Currency.GetAllCurrencies()
                .ToDictionary(x => x.Code, x => x.Symbol);
        }
    }
}