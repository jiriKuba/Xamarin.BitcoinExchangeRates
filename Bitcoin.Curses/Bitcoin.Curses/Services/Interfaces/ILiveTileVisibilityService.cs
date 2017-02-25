using Bitcoin.Curses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services.Interfaces
{
    public interface ILiveTileVisibilityService
    {
        void SetExchangeRateVisibleOnLiveTile(string exchangeRateKey, bool isVisible);

        void AddLiveTileVisibilityToModel(Dictionary<string, BitcoinExchangeRate> models);
    }
}
