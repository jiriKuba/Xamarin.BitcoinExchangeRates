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
    public class ExchangeRateViewModel : ViewModelBase
    {
        private BitcoinExchangeRate _exchangeRate;
        private readonly string _currencyCode;
        private readonly MainViewModel _mainViewModel;
        private readonly IBitcoinDataService _bitcoinDataService;
        private readonly IRateSettingsApplyService _rateSettingsApplyService;
        private readonly ICustomCurrencySymbolServise _customCurrencyCodeServise;

        public ExchangeRateViewModel(string currencyCode, BitcoinExchangeRate exchangeRate)
        {
            _exchangeRate = exchangeRate;
            _currencyCode = currencyCode == null ? string.Empty : currencyCode;
            _mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            _bitcoinDataService = ServiceLocator.Current.GetInstance<IBitcoinDataService>();
            _rateSettingsApplyService = ServiceLocator.Current.GetInstance<IRateSettingsApplyService>();
            _customCurrencyCodeServise = ServiceLocator.Current.GetInstance<ICustomCurrencySymbolServise>();
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

        public decimal? MarketPriceDifference
        {
            get
            {
                if (_exchangeRate != null)
                    return _exchangeRate.YesterdayRate == 0 ? 0 : (_exchangeRate.RecentMarketPrice - _exchangeRate.YesterdayRate);
                else return null;
            }
        }

        public decimal MarketPriceDifferencePercentage
        {
            get
            {
                if (_exchangeRate != null)
                {
                    if (_exchangeRate.YesterdayRate == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        var firstVal = _exchangeRate.RecentMarketPrice > _exchangeRate.YesterdayRate ? _exchangeRate.RecentMarketPrice : _exchangeRate.YesterdayRate;
                        var secVal = firstVal == _exchangeRate.RecentMarketPrice ? _exchangeRate.YesterdayRate : _exchangeRate.RecentMarketPrice;
                        if (secVal == 0)
                        {
                            return 0;
                        }

                        return (firstVal * 100 / secVal) - 100;
                    }
                }
                else return 0;
            }
        }

        public string MarketPriceDifferenceLabel
        {
            get
            {
                if (_exchangeRate != null)
                {
                    if (MarketPriceDifference.HasValue)
                    {
                        var rateValue = MarketPriceDifference.Value;
                        if (rateValue >= 0)
                        {
                            return "+ " + rateValue.ToString("N2") + $" ({MarketPriceDifferencePercentage.ToString("N2")}%)";
                        }
                        else if (rateValue < 0)
                        {
                            return "- " + Math.Abs(rateValue).ToString("N2") + $" ({MarketPriceDifferencePercentage.ToString("N2")}%)";
                        }
                        else
                        {
                            return "+ " + decimal.Zero.ToString("N2") + " (0%)";
                        }
                    }
                    else
                    {
                        return "+ " + decimal.Zero.ToString("N2") + " (0%)";
                    }
                }
                else return "+ " + decimal.Zero.ToString("N2") + " (0%)";
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
                    if (IsCurrencySymbolOnStart)
                    {
                        return string.Format("{1}{0}", DelayedMarketPrice.HasValue ? DelayedMarketPrice.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
                    else
                    {
                        return string.Format("{0}{1}", DelayedMarketPrice.HasValue ? DelayedMarketPrice.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
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
                    if (IsCurrencySymbolOnStart)
                    {
                        return string.Format("{1}{0}", RecentMarketPrice.HasValue ? RecentMarketPrice.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
                    else
                    {
                        return string.Format("{0}{1}", RecentMarketPrice.HasValue ? RecentMarketPrice.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
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
                    if (IsCurrencySymbolOnStart)
                    {
                        return string.Format("{1}{0}", Buy.HasValue ? Buy.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
                    else
                    {
                        return string.Format("{0}{1}", Buy.HasValue ? Buy.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
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
                    if (IsCurrencySymbolOnStart)
                    {
                        return string.Format("{1}{0}", Sell.HasValue ? Sell.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
                    else
                    {
                        return string.Format("{0}{1}", Sell.HasValue ? Sell.Value.ToString("N2") : string.Empty, CurrencySymbol);
                    }
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
                _rateSettingsApplyService.SetExchangeRateVisibleOnLiveTile(CurrencyCode, value);
                _exchangeRate.IsVisibleOnLiveTile = value;
                RaisePropertyChanged(() => ShowRateInLiveTile);
            }
        }

        public bool IsCurrencySymbolOnStart
        {
            get
            {
                return _exchangeRate.IsCurrencySymbolOnStart;
            }
            set
            {
                _rateSettingsApplyService.SetExchangeRateCurrencySymbolOnStart(CurrencyCode, value);
                _exchangeRate.IsCurrencySymbolOnStart = value;
                RaisePropertyChanged(() => IsCurrencySymbolOnStart);
                RaisePropertyChanged(() => SellLabel);
                RaisePropertyChanged(() => BuyLabel);
                RaisePropertyChanged(() => RecentMarketPriceLabel);
                RaisePropertyChanged(() => DelayedMarketPriceLabel);
                RaisePropertyChanged(() => ExchangeRateLabel);
                RaisePropertyChanged(() => MarketPriceDifferenceLabel);
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
            RaisePropertyChanged(() => MarketPriceDifferenceLabel);
        }
    }
}