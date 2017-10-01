using Bitcoin.Curses.Services.Interfaces;
using Bitcoin.Curses.ViewModel;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class CurrencyNavigateService : ICurrencyNavigateService
    {
        private readonly MainViewModel _mainViewModel;

        public CurrencyNavigateService(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public void NavigateToCurrency(string currencyCode)
        {
            _mainViewModel.ExchangeRateDetail = _mainViewModel.ExchangeRates.ExchangeRateList
                .FirstOrDefault(x => x.CurrencyCode == currencyCode);
        }
    }
}