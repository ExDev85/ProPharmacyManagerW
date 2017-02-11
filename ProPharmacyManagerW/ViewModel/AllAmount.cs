// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Windows;
using System.Windows.Data;

namespace ProPharmacyManagerW.ViewModel
{
    public class AllAmount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (System.Convert.ToByte(value) == 0)
                {
                    return "#DA4453";
                }
                else if (System.Convert.ToByte(value) > 0 && System.Convert.ToByte(value) <= 5)
                {
                    return "#E9573F";
                }
                else
                {
                    return DependencyProperty.UnsetValue;
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