using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Bitcoin.Curses.Messages;
using Bitcoin.Curses.Models;
using Bitcoin.Curses.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Bitcoin.Curses.Extensions;

namespace Bitcoin.Curses.ViewModel
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class MainViewModel : ViewModelBase
    {
        private readonly IBitcoinDataService _bitcoinDataService;
        private readonly MainModel _mainModel;

        public MainViewModel(IBitcoinDataService bitcoinDataService)
        {
            _bitcoinDataService = bitcoinDataService;
            _mainModel = new MainModel();
            RefreshCommand = new RelayCommand(DoRefreshCommand);
            OpenCoindeskUrlCommand = new RelayCommand(DoOpenCoindeskUrlCommand);
            OpenBlockchainUrlCommand = new RelayCommand(DoOpenBlockchainUrlCommand);
        }

        public RelayCommand RefreshCommand { get; private set; }

        public RelayCommand OpenCoindeskUrlCommand { get; private set; }

        public RelayCommand OpenBlockchainUrlCommand { get; private set; }

        private ExchangeRatesViewModel _exchangeRates;

        public ExchangeRatesViewModel ExchangeRates
        {
            get
            {
                if (_exchangeRates == null)
                {
                    _exchangeRates = new ExchangeRatesViewModel(_mainModel.ExchangeRates);
                }

                return _exchangeRates;
            }
            set
            {
                _exchangeRates = value;
                base.RaisePropertyChanged(() => ExchangeRates);
                base.RaisePropertyChanged(() => GeneratedText);
            }
        }

        public bool IsWindowsUWPMobile
        {
            get { return Device.Idiom == TargetIdiom.Phone && Device.RuntimePlatform == Device.UWP; }
        }

        public bool IsNotWindowsUWPMobile
        {
            get { return !IsWindowsUWPMobile; }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged(() => SearchText);
                SearchExchangeRate();
            }
        }

        public bool IsSearchTextSet
        {
            get { return !string.IsNullOrEmpty(SearchText); }
        }

        private async void SearchExchangeRate()
        {
            ShowProgressBar = true;
            var searchChanges = IsSearchTextSet
                ? new ExchangeRates(_mainModel.ExchangeRates.ExchangeRateList.FilterByCurrencyCode(SearchText))
                : _mainModel.ExchangeRates;
            ExchangeRates = await LoadExchangeRatesViewModel(searchChanges);

            if (ExchangeRates != null)
            {
                ShowProgressBar = false;
            }

            Messenger.Default.Send<ExchangeRatesLoadedMessage>(new ExchangeRatesLoadedMessage());
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
            }
        }

        public bool IsUIEnabled
        {
            get
            {
                return !_showProgressBar;
            }
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

            //since Xamarin.Forms 2.4 must be this otherwise selected ExchangeRateDetail not change after refresh
            ExchangeRateDetail = null;

            _mainModel.ExchangeRates = await _bitcoinDataService.GetExchangeRatesAsync();
            ExchangeRates = await LoadExchangeRatesViewModel(_mainModel.ExchangeRates);

            ShowProgressBar = false;

            Messenger.Default.Send<ExchangeRatesLoadedMessage>(new ExchangeRatesLoadedMessage());
        }

        private Task<ExchangeRatesViewModel> LoadExchangeRatesViewModel(ExchangeRates rates)
        {
            return Task.Run(() =>
            {
                return new ExchangeRatesViewModel(rates);
            });
        }

        public void DoRefreshCommand()
        {
            if (!ShowProgressBar)
            {
                if (ExchangeRateDetail != null)
                {
                    ExchangeRateDetail.Clear();
                }
                LoadData();
            }
        }

        public void DoMainPageLoadCommand()
        {
            LoadData();
        }

        public void DoOpenCoindeskUrlCommand()
        {
            OpenUrl("https://www.coindesk.com/");
        }

        public void DoOpenBlockchainUrlCommand()
        {
            OpenUrl("https://blockchain.info/");
        }

        public override void Cleanup()
        {
            ExchangeRateDetail = null;
            if (ExchangeRates != null)
                ExchangeRates.Cleanup();
            base.Cleanup();
        }

        public void OpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }
    }
}