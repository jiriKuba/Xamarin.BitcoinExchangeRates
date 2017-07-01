using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services.Interfaces
{
    public interface ICurrencyNavigateService
    {
        void NavigateToCurrency(string currencyCode);
    }
}