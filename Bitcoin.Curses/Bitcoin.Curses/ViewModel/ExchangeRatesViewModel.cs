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
    public class ExchangeRatesViewModel : ViewModelBase
    {
        private readonly ExchangeRates _exchangeRates;
        private readonly ObservableCollection<ExchangeRateViewModel> _exchangeRateList;
        public ObservableCollection<ExchangeRateViewModel> ExchangeRateList
        {
            get
            {
                return this._exchangeRateList;
            }
        }
        
        public DateTime? Generated
        {
            get
            {
                if (this._exchangeRates != null)
                    return this._exchangeRates.Generated;
                else return null;
            }
        }

        private ExchangeRatesViewModel()
        {
            this._exchangeRateList = new ObservableCollection<ExchangeRateViewModel>();
        }

        public ExchangeRatesViewModel(ExchangeRates exchangeRates)
            :this()
        {
            this._exchangeRates = exchangeRates;

            if (exchangeRates != null && exchangeRates.ExchangeRateList != null && exchangeRates.ExchangeRateList.Count > 0)
            {
                foreach (KeyValuePair<String, ExchangeRate> item in exchangeRates.ExchangeRateList)
                {
                    this._exchangeRateList.Add(new ExchangeRateViewModel(item.Key, item.Value));
                }
            }
        }

        public override void Cleanup()
        {
            this._exchangeRateList.Clear();

            base.Cleanup();
        }
    }
}
