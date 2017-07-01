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
    public class CurrencyNavigateService : ICurrencyNavigateService
    {
        private readonly MainViewModel _mainViewModel;

        public CurrencyNavigateService()
        {
            _mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
        }

        public void NavigateToCurrency(string currencyCode)
        {
            var exchangeRate = _mainViewModel.ExchangeRates.ExchangeRateList
                .FirstOrDefault(x => x.CurrencyCode == currencyCode);

            if (exchangeRate != null)
            {
                _mainViewModel.ExchangeRateDetail = exchangeRate;
            }
        }
    }
}