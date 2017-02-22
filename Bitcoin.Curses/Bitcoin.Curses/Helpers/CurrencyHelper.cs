using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Helpers
{
    public class CurrencyHelper
    {
        public Dictionary<string, string> CurrencySymbols { get; private set; }

        public CurrencyHelper()
        {
            CurrencySymbols = new Dictionary<string, string>();
            CurrencySymbols.Add("CZK", "Kč");
        }
    }
}
