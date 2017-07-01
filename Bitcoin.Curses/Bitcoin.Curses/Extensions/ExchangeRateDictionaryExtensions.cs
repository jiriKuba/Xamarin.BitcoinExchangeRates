using Bitcoin.Curses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Extensions
{
    public static class ExchangeRateDictionaryExtensions
    {
        public static IDictionary<string, BitcoinExchangeRate> FilterByCurrencyCode(this IDictionary<string, BitcoinExchangeRate> models, string searchText)
        {
            if (models == null || string.IsNullOrEmpty(searchText))
            {
                return models;
            }
            else
            {
                return models.Where(x => x.Key.ToLower().Contains(searchText.ToLower()) || x.Value.CurrencySymbol.ToLower().Contains(searchText.ToLower()))
                    .ToDictionary(x => x.Key, y => y.Value);
            }
        }
    }
}