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
    internal class ExchangeRateViewModel : ViewModelBase
    {
        private BitcoinExchangeRate _exchangeRate;
        private readonly string _currencyCode;
        private readonly MainViewModel _mainViewModel;
        private readonly IBitcoinDataService _bitcoinDataService;
        private readonly ILiveTileVisibilityService _liveTileVisibilityService;
        private readonly ICustomCurrencyCodeServise _customCurrencyCodeServise;

        public ExchangeRateViewModel(string currencyCode, BitcoinExchangeRate exchangeRate)
        {
            _exchangeRate = exchangeRate;
            _currencyCode = currencyCode == null ? string.Empty : currencyCode;
            _mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            _bitcoinDataService = ServiceLocator.Current.GetInstance<IBitcoinDataService>();
            _liveTileVisibilityService = ServiceLocator.Current.GetInstance<ILiveTileVisibilityService>();
            _customCurrencyCodeServise = ServiceLocator.Current.GetInstance<ICustomCurrencyCodeServise>();
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

        public decimal? MarketPriceDiference
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.YesterdayRate == 0 ? 0 : (_exchangeRate.RecentMarketPrice - _exchangeRate.YesterdayRate);
                else return null;
            }
        }

        public string MarketPriceDiferenceLabel
        {
            get
            {
                if (_exchangeRate != null)
                {
                    if (MarketPriceDiference.HasValue)
                    {
                        var rounded = Math.Round(MarketPriceDiference.Value, 2);
                        if (rounded >= 0)
                        {
                            return "+ " + rounded.ToString("N2");
                        }
                        else if (rounded < 0)
                        {
                            return "- " + Math.Abs(rounded).ToString("N2");
                        }
                        else
                        {
                            return "+ " + decimal.Zero.ToString("N2");
                        }
                    }
                    else
                    {
                        return "+ " + decimal.Zero.ToString("N2");
                    }
                }
                else return "+ " + decimal.Zero.ToString("N2");
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
                else return string.Empty;
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
                else return string.Empty;
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

        public decimal? YesterdayRate
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.YesterdayRate;
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
                else return string.Empty;
            }
        }

        public string CurrencySymbol
        {
            get
            {
                if (_exchangeRate != null)
                {
                    if (!string.IsNullOrEmpty(CustomCurrencySymbol))
                    {
                        return CustomCurrencySymbol;
                    }
                    else return _exchangeRate.CurrencySymbol == null ? string.Empty : _exchangeRate.CurrencySymbol;
                }
                else return string.Empty;
            }
        }

        public string CustomCurrencySymbol
        {
            get
            {
                if (_exchangeRate != null)
                {
                    return _exchangeRate.CustomCurrencySymbol;
                }
                else return string.Empty;
            }
            set
            {
                if (_exchangeRate != null)
                {
                    _exchangeRate.CustomCurrencySymbol = value;
                }
                _customCurrencyCodeServise.SetExchangeRateCustomCurrencySymbol(CurrencyCode, value);

                RaisePropertyChanged(() => CustomCurrencySymbol);
                RaisePropertyChanged(() => CurrencySymbol);
                RaisePropertyChanged(() => SellLabel);
                RaisePropertyChanged(() => BuyLabel);
                RaisePropertyChanged(() => RecentMarketPriceLabel);
                RaisePropertyChanged(() => DelayedMarketPriceLabel);
                RaisePropertyChanged(() => ExchangeRateLabel);
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

        public void Clear()
        {
            _exchangeRate = null;
            RaisePropertyChanged(() => CustomCurrencySymbol);
            RaisePropertyChanged(() => CurrencySymbol);
            RaisePropertyChanged(() => SellLabel);
            RaisePropertyChanged(() => BuyLabel);
            RaisePropertyChanged(() => RecentMarketPriceLabel);
            RaisePropertyChanged(() => DelayedMarketPriceLabel);
            RaisePropertyChanged(() => ExchangeRateLabel);
            RaisePropertyChanged(() => MarketPriceDiferenceLabel);
        }
    }
}