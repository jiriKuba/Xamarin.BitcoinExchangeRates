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
    internal class ExchangeRatesViewModel : ViewModelBase
    {
        private readonly ExchangeRates _exchangeRates;
        private readonly ObservableCollection<ExchangeRateViewModel> _exchangeRateList;
        private string _searchText;
        private ObservableCollection<ExchangeRateViewModel> _searchList;

        public ObservableCollection<ExchangeRateViewModel> ExchangeRateList
        {
            get
            {
                if (!string.IsNullOrEmpty(_searchText))
                {
                    CleanSearchList();
                    _searchList = new ObservableCollection<ExchangeRateViewModel>(_exchangeRateList
                        .Where(x => x.CurrencyCode.ToLower().Contains(_searchText.ToLower()) || x.CurrencySymbol.ToLower().Contains(_searchText.ToLower())));

                    return _searchList;
                }
                else return _exchangeRateList;
            }
        }

        public DateTime? Generated
        {
            get
            {
                if (_exchangeRates != null)
                    return _exchangeRates.Generated;
                else return null;
            }
        }

        private ExchangeRatesViewModel()
        {
            _exchangeRateList = new ObservableCollection<ExchangeRateViewModel>();
        }

        public ExchangeRatesViewModel(ExchangeRates exchangeRates)
            : this()
        {
            _exchangeRates = exchangeRates;

            if (exchangeRates != null && exchangeRates.ExchangeRateList != null && exchangeRates.ExchangeRateList.Count > 0)
            {
                foreach (KeyValuePair<string, BitcoinExchangeRate> item in exchangeRates.ExchangeRateList)
                {
                    _exchangeRateList.Add(new ExchangeRateViewModel(item.Key, item.Value));
                }
            }
        }

        public ExchangeRatesViewModel ResetSearch()
        {
            _searchText = null;
            //base.RaisePropertyChanged(() => ExchangeRateList);
            return this;
        }

        public override void Cleanup()
        {
            _exchangeRateList.Clear();
            CleanSearchList();

            base.Cleanup();
        }

        public ExchangeRatesViewModel Search(string searchText)
        {
            _searchText = searchText;
            //base.RaisePropertyChanged(() => ExchangeRateList);
            return this;
        }

        private void CleanSearchList()
        {
            if (_searchList != null)
            {
                _searchList.Clear();
            }
        }
    }
}