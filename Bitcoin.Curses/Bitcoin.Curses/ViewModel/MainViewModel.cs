using Bitcoin.Curses.Messages;
using Bitcoin.Curses.Models;
using Bitcoin.Curses.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bitcoin.Curses.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IBitcoinDataService _bitcoinDataService;
        private readonly MainModel _mainModel;

        public MainViewModel(IBitcoinDataService bitcoinDataService)
        {
            _bitcoinDataService = bitcoinDataService;
            _mainModel = new MainModel();
            //this.MainPageLoadCommand = new RelayCommand(this.DoMainPageLoadCommand);

            //ExchangeRates mock = new ExchangeRates();
            //mock.ExchangeRateList.Add("USD", new ExchangeRate() { DelayedMarketPrice = 478.68M, RecentMarketPrice = 478.68M, Buy = 478.55M, Sell = 478.68M, CurrencySymbol = "$" });
            //this._mainModel.ExchangeRates = mock;
            //this.ExchangeRates = new ExchangeRatesViewModel(this._mainModel.ExchangeRates);
        }

        //public RelayCommand MainPageLoadCommand { get; private set; }

        private ExchangeRatesViewModel _exchangeRates;

        public ExchangeRatesViewModel ExchangeRates
        {
            get
            {
                if (_exchangeRates == null)
                    _exchangeRates = new ExchangeRatesViewModel(_mainModel.ExchangeRates);
                return _exchangeRates;
            }
            set
            {
                _exchangeRates = value;
                base.RaisePropertyChanged(() => ExchangeRates);
                base.RaisePropertyChanged(() => GeneratedText);
            }
        }

        private bool _showProgressBar;

        public bool ShowProgressBar
        {
            get
            {
                return _showProgressBar;
            }
            set
            {
                _showProgressBar = value;
                RaisePropertyChanged(() => ShowProgressBar);
                //base.RaisePropertyChanged(() => this.IsUIEnabled);
            }
        }

        public bool IsUIEnabled
        {
            get
            {
                return !_showProgressBar;
            }
            //set
            //{
            //    this._showProgressBar = !value;
            //    base.RaisePropertyChanged(() => this.IsUIEnabled);
            //}
        }

        public string GeneratedText
        {
            get
            {
                if (_exchangeRates != null && _exchangeRates.Generated.HasValue)
                    return "Loaded: " + _exchangeRates.Generated.Value.ToString();
                else return string.Empty;
            }
        }

        private ExchangeRateViewModel _exchangeRateDetail;

        public ExchangeRateViewModel ExchangeRateDetail
        {
            get
            {
                return _exchangeRateDetail;
            }
            set
            {
                _exchangeRateDetail = value;
                RaisePropertyChanged(() => ExchangeRateDetail);
            }
        }

        private async void LoadData()
        {
            ShowProgressBar = true;

            _mainModel.ExchangeRates = await _bitcoinDataService.GetExchangeRatesAsync();
            ExchangeRates = await LoadExchangeRatesViewModel(_mainModel.ExchangeRates);

            if (ExchangeRates != null)
            {
                ShowProgressBar = false;
            }

            Messenger.Default.Send<ExchangeRatesLoadedMessage>(new ExchangeRatesLoadedMessage());
        }

        private Task<ExchangeRatesViewModel> LoadExchangeRatesViewModel(ExchangeRates rates)
        {
            return Task.Run(() =>
            {
                return new ExchangeRatesViewModel(rates);
            });
        }

        public void DoMainPageLoadCommand()
        {
            LoadData();
        }

        public override void Cleanup()
        {
            ExchangeRateDetail = null;
            if (ExchangeRates != null)
                ExchangeRates.Cleanup();
            base.Cleanup();
        }
    }
}