using Bitcoin.Curses.Messages;
using Bitcoin.Curses.Services.Interfaces;
using Bitcoin.Curses.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bitcoin.Curses
{
    public partial class MainPage : MasterDetailPage
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ICurrencyNavigateService _currencyNavigateService;

        public MainPage()
        {
            InitializeComponent();
            _mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            _currencyNavigateService = ServiceLocator.Current.GetInstance<ICurrencyNavigateService>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _mainViewModel.DoMainPageLoadCommand();
            Messenger.Default.Register<ExchangeRatesLoadedMessage>(this, (action) => SelectLastExchangeRate(action));
        }

        private void SelectLastExchangeRate(ExchangeRatesLoadedMessage message)
        {
            string selectedCurrency = Settings.GetLastViewedCurrency();
            _currencyNavigateService.NavigateToCurrency(selectedCurrency);
        }

        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ExchangeRateViewModel item = e.SelectedItem as ExchangeRateViewModel;
            if (item != null)
            {
                Settings.SetLastViewedCurrency(item.CurrencyCode);
                _currencyNavigateService.NavigateToCurrency(item.CurrencyCode);
                this.IsPresented = false;
            }
        }
    }
}