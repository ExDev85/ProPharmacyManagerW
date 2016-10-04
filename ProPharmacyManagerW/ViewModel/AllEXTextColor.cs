// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ProPharmacyManagerW.ViewModel
{
    class AllEXTextColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var d1 = DateTime.Now.Date;
                var d2 = System.Convert.ToDateTime(value).Date;
                var dt = (d1 - d2).TotalDays;
                if (dt > -5)
                {
                    return Brushes.White;
                }
                else
                {
                    return Brushes.Blue;
                }
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}