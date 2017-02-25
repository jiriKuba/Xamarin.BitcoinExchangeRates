using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services.Interfaces
{
    public interface IDataProvideService
    {
        Task<String> GetBitcoinJSONData();

        Task<String> GetExchangeJSONData();
    }
}
