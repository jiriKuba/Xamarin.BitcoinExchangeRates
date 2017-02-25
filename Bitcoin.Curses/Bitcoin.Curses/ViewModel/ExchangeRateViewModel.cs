using Bitcoin.Curses.Models;
using Bitcoin.Curses.Services.Interfaces;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.ViewModel
{
    class ExchangeRateViewModel : ViewModelBase
    {
        private readonly BitcoinExchangeRate _exchangeRate;
        private readonly String _currencyCode;
        private readonly MainViewModel _mainViewModel;
        private readonly IBitcoinDataService _bitcoinDataService;
        private readonly ILiveTileVisibilityService _liveTileVisibilityService;

        public ExchangeRateViewModel(string currencyCode, BitcoinExchangeRate exchangeRate)
        {
            _exchangeRate = exchangeRate;
            _currencyCode = currencyCode;
            _mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            _bitcoinDataService = ServiceLocator.Current.GetInstance<IBitcoinDataService>();
            _liveTileVisibilityService = ServiceLocator.Current.GetInstance<ILiveTileVisibilityService>();
        }

        public string CurrencyCode
        {
            get
            {
                return _currencyCode;
            }
        }

        public string ExchangeRateLabel
        {
            get
            {
                if (!string.IsNullOrEmpty(_currencyCode) && !string.IsNullOrEmpty(CurrencySymbol))
                {
                    return string.Format("{0}[{1}]", _currencyCode.ToUpper(), CurrencySymbol);
                }
                else return CurrencyCode;
            }
        }

        public decimal? DelayedMarketPrice
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.DelayedMarketPrice;
                else return null;
            }
        }

        public string DelayedMarketPriceLabel
        {
            get
            {
                if (_exchangeRate != null)
                {
                    return string.Format("{0}{1}", DelayedMarketPrice.HasValue ? Math.Round(DelayedMarketPrice.Value, 2).ToString("N2") : string.Empty, CurrencySymbol);
                }
                else return null;
            }
        }

        public decimal? RecentMarketPrice
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.RecentMarketPrice;
                else return null;
            }
        }

        public string RecentMarketPriceLabel
        {
            get
            {
                if (_exchangeRate != null)
                {
                    return string.Format("{0}{1}", RecentMarketPrice.HasValue ? Math.Round(RecentMarketPrice.Value, 2).ToString("N2") : string.Empty, CurrencySymbol);
                }
                else return null;
            }
        }

        public decimal? Buy
        {
            get
            {
                if (this._exchangeRate != null)
                    return this._exchangeRate.Buy;
                else return null;
            }
        }

        public string BuyLabel
        {
            get
            {
                if (_exchangeRate != null)
                {
                    return string.Format("{0}{1}", Buy.HasValue ? Math.Round(Buy.Value, 2).ToString("N2") : string.Empty, CurrencySymbol);
                }
                else return null;
            }
        }

        public decimal? Sell
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.Sell;
                else return null;
            }
        }

        public string SellLabel
        {
            get
            {
                if (_exchangeRate != null)
                {
                    return string.Format("{0}{1}", Sell.HasValue ? Math.Round(Sell.Value, 2).ToString("N2") : string.Empty, CurrencySymbol);
                }
                else return null;
            }
        }

        public string CurrencySymbol
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.CurrencySymbol;
                else return null;
            }
        }

        public DateTime? LoadedTime
        {
            get
            {
                if (_mainViewModel != null && _mainViewModel.ExchangeRates != null)
                    return _mainViewModel.ExchangeRates.Generated;
                else return null;
            }
        }

        public bool ShowRateInLiveTile
        {
            get
            {
                return _exchangeRate.IsVisibleOnLiveTile;
            }
            set
            {
                _liveTileVisibilityService.SetExchangeRateVisibleOnLiveTile(CurrencyCode, value);
                _exchangeRate.IsVisibleOnLiveTile = value;
                RaisePropertyChanged(() => ShowRateInLiveTile);
            }
        }
    }
}
