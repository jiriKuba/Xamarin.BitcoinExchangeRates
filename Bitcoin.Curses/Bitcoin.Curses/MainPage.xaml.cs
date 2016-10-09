using Bitcoin.Curses.ViewModel;
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
        }

        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ExchangeRateViewModel item = e.SelectedItem as ExchangeRateViewModel;
            if (item != null)
            {
                ServiceLocator.Current.GetInstance<MainViewModel>().ExchangeRateDetail = item;
                this.IsPresented = false;
            }
        }
    }
}
