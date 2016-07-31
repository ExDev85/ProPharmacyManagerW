// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace ProPharmacyManagerW.ViewModel
{
    class NoDecimal : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var newValue = value.ToString().Replace(".00", "");
                return newValue;
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                return "0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}