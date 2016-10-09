﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Models
{
    class MainModel
    {
        public ExchangeRates ExchangeRates { get; set; }

        public MainModel()
        {

        }

        public MainModel(ExchangeRates exchangeRates)
            :this()
        {
            this.ExchangeRates = exchangeRates;
        }
    }
}