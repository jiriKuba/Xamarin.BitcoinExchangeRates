using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services.Interfaces
{
    public interface IDataProvideService
    {
        Task<string> GetBitcoinJSONData();

        Task<string> GetSpotBitcoinJSONDataFromUSD();

        Task<string> GetExchangeJSONData();

        Task<string> GetHistoryJSONData();
    }
}