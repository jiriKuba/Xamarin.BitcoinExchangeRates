using Bitcoin.Curses.Models;
using Bitcoin.Curses.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private readonly IBitcoinDataService _bitcoinDataService;

        private MainModel _mainModel;

        public MainViewModel(IBitcoinDataService bitcoinDataService)
        {
            this._bitcoinDataService = bitcoinDataService;
            this._mainModel = new MainModel();
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
                if (this._exchangeRates == null)
                    this._exchangeRates = new ExchangeRatesViewModel(this._mainModel.ExchangeRates);
                return this._exchangeRates;
            }
            set
            {
                this._exchangeRates = value;
                base.RaisePropertyChanged(() => this.ExchangeRates);
                base.RaisePropertyChanged(() => this.GeneratedText);
            }
        }

        private Boolean _showProgressBar;
        public Boolean ShowProgressBar
        {
            get
            {
                return this._showProgressBar;
            }
            set
            {
                this._showProgressBar = value;
                base.RaisePropertyChanged(() => this.ShowProgressBar);
                //base.RaisePropertyChanged(() => this.IsUIEnabled);
            }
        }
        
        public Boolean IsUIEnabled
        {
            get
            {
                return !this._showProgressBar;
            }
            //set
            //{
            //    this._showProgressBar = !value;
            //    base.RaisePropertyChanged(() => this.IsUIEnabled);
            //}
        }

        public String GeneratedText
        {
            get
            {
                if (this._exchangeRates != null && this._exchangeRates.Generated.HasValue)
                    return "Loaded: " + this._exchangeRates.Generated.Value.ToString();
                else return String.Empty;
            }
        }

        private ExchangeRateViewModel _exchangeRateDetail;
        public ExchangeRateViewModel ExchangeRateDetail
        {
            get
            {
                return this._exchangeRateDetail;
            }
            set
            {
                this._exchangeRateDetail = value;
                base.RaisePropertyChanged(() => this.ExchangeRateDetail);
            }
        }

        private async void LoadData()
        {
            this.ShowProgressBar = true;

            this._mainModel.ExchangeRates = await this._bitcoinDataService.GetExchangeRatesAsync();            
            this.ExchangeRates = new ExchangeRatesViewModel(this._mainModel.ExchangeRates);

            this.ShowProgressBar = false;
        }

        public void DoMainPageLoadCommand()
        {
            this.LoadData();
        }

        public override void Cleanup()
        {
            this.ExchangeRateDetail = null;
            if (this.ExchangeRates != null)
                this.ExchangeRates.Cleanup();
            base.Cleanup();
        }
    }
}
