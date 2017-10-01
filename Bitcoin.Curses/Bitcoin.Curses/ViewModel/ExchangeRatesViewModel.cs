using Bitcoin.Curses.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.ViewModel
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ExchangeRatesViewModel : ViewModelBase
    {
        public ObservableCollection<ExchangeRateViewModel> ExchangeRateList { get; private set; }

        public readonly DateTime? Generated;

        private ExchangeRatesViewModel()
        {
            ExchangeRateList = new ObservableCollection<ExchangeRateViewModel>();
        }

        public ExchangeRatesViewModel(ExchangeRates exchangeRates)
            : this()
        {
            Generated = exchangeRates?.Generated;

            if (exchangeRates != null && exchangeRates.ExchangeRateList != null)
            {
                foreach (var item in exchangeRates.ExchangeRateList)
                {
                    ExchangeRateList.Add(new ExchangeRateViewModel(item.Key, item.Value));
                }
            }
        }

        public override void Cleanup()
        {
            ExchangeRateList.Clear();
            base.Cleanup();
        }
    }
}