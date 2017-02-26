using Bitcoin.Curses.Messages;
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
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ServiceLocator.Current.GetInstance<MainViewModel>().DoMainPageLoadCommand();
            Messenger.Default.Register<ExchangeRatesLoadedMessage>(this, (action) => SelectLastExchangeRate(action));
        }

        private void SelectLastExchangeRate(ExchangeRatesLoadedMessage message)
        {
            string selectedCurrency = Settings.GetLastViewedCurrency();

            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            var exchangeViewModel = mainViewModel.ExchangeRates
                .ExchangeRateList
                .Where(x => x.CurrencyCode == selectedCurrency)
                .FirstOrDefault();

            mainViewModel.ExchangeRateDetail = exchangeViewModel;
        }

        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ExchangeRateViewModel item = e.SelectedItem as ExchangeRateViewModel;
            if (item != null)
            {
                Settings.SetLastViewedCurrency(item.CurrencyCode);
                ServiceLocator.Current.GetInstance<MainViewModel>().ExchangeRateDetail = item;
                this.IsPresented = false;
            }
        }
    }
}
