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

        public ExchangeRateViewModel(String currencyCode, BitcoinExchangeRate exchangeRate)
        {
            this._exchangeRate = exchangeRate;
            this._currencyCode = currencyCode;
            this._mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            this._bitcoinDataService = ServiceLocator.Current.GetInstance<IBitcoinDataService>();
        }

        public String CurrencyCode
        {
            get
            {
                return this._currencyCode;
            }
        }

        public String ExchangeRateLabel
        {
            get
            {
                if (!String.IsNullOrEmpty(this._currencyCode) && !String.IsNullOrEmpty(CurrencySymbol))
                {
                    return String.Format("{0}[{1}]", this._currencyCode.ToUpper(), this.CurrencySymbol);
                }
                else return this.CurrencyCode;
            }
        }

        public Decimal? DelayedMarketPrice
        {
            get
            {
                if (this._exchangeRate != null)
                    return this._exchangeRate.DelayedMarketPrice;
                else return null;
            }
        }

        public String DelayedMarketPriceLabel
        {
            get
            {
                if (this._exchangeRate != null)
                {
                    return String.Format("{0}{1}", this.DelayedMarketPrice.HasValue ? Math.Round(this.DelayedMarketPrice.Value, 2).ToString("N2") : String.Empty, this.CurrencySymbol);
                }
                else return null;
            }
        }

        public Decimal? RecentMarketPrice
        {
            get
            {
                if (this._exchangeRate != null)
                    return this._exchangeRate.RecentMarketPrice;
                else return null;
            }
        }

        public String RecentMarketPriceLabel
        {
            get
            {
                if (this._exchangeRate != null)
                {
                    return String.Format("{0}{1}", this.RecentMarketPrice.HasValue ? Math.Round(this.RecentMarketPrice.Value, 2).ToString("N2") : String.Empty, this.CurrencySymbol);
                }
                else return null;
            }
        }

        public Decimal? Buy
        {
            get
            {
                if (this._exchangeRate != null)
                    return this._exchangeRate.Buy;
                else return null;
            }
        }

        public String BuyLabel
        {
            get
            {
                if (this._exchangeRate != null)
                {
                    return String.Format("{0}{1}", this.Buy.HasValue ? Math.Round(this.Buy.Value, 2).ToString("N2") : String.Empty, this.CurrencySymbol);
                }
                else return null;
            }
        }

        public Decimal? Sell
        {
            get
            {
                if (this._exchangeRate != null)
                    return this._exchangeRate.Sell;
                else return null;
            }
        }

        public String SellLabel
        {
            get
            {
                if (this._exchangeRate != null)
                {
                    return String.Format("{0}{1}", this.Sell.HasValue ? Math.Round(this.Sell.Value, 2).ToString("N2") : String.Empty, this.CurrencySymbol);
                }
                else return null;
            }
        }

        public String CurrencySymbol
        {
            get
            {
                if (this._exchangeRate != null)
                    return this._exchangeRate.CurrencySymbol;
                else return null;
            }
        }

        public DateTime? LoadedTime
        {
            get
            {
                if (this._mainViewModel != null && this._mainViewModel.ExchangeRates != null)
                    return this._mainViewModel.ExchangeRates.Generated;
                else return null;
            }
        }

        public Boolean ShowRateInLiveTile
        {
            get
            {
                return this._exchangeRate.IsVisibleOnLiveTile;
            }
            set
            {
                this._bitcoinDataService.SetExchangeRateVisibleOnLiveTile(this.CurrencyCode, value);
                this._exchangeRate.IsVisibleOnLiveTile = value;
                base.RaisePropertyChanged(() => this.ShowRateInLiveTile);
            }
        }
    }
}
