﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BCMS.Models
{
    public class CurrencyConverter
    {
        private readonly double EUR = 1.49, CNY = 0.172175, AUD = 1.00;
        /*
         * Converts the foreign currency amount into AUD.
         */
        public double ConvertCurrencyToAUD(String ConvertType, double money)
        {
            double conversion = 0.0;

            if (ConvertType == "EUR")
                conversion = money * EUR;
            else if (ConvertType == "CNY")
                conversion = money * CNY;
            else if (ConvertType == "AUD")
                conversion = money * AUD;

            return conversion;
        }
    }
}