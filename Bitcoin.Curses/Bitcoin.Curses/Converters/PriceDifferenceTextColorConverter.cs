using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bitcoin.Curses.Converters
{
    public class PriceDifferenceTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var stringValue = value as String;
                if (string.IsNullOrEmpty(stringValue))
                {
                    return GetColorByResourceKey("PriceStillColor");
                }
                else if (stringValue.StartsWith("+"))
                {
                    return GetColorByResourceKey("PriceUpColor");
                }
                else if (stringValue.StartsWith("-"))
                {
                    return GetColorByResourceKey("PriceDownColor");
                }
            }

            return GetColorByResourceKey("PriceStillColor");
        }

        private Color GetColorByResourceKey(string resourceKey)
        {
            object color = null;
            if (App.Instance.Resources.TryGetValue(resourceKey, out color))
            {
                return (Color)color;
            }
            else
            {
                return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}