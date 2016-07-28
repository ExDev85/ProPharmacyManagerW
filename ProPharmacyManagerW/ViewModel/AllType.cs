// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Windows.Data;

namespace ProPharmacyManagerW.ViewModel
{
    public class AllType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                switch (value.ToString())
                {
                    case "1":
                        return "شرب";
                    case "2":
                        return "اقراص";
                    case "3":
                        return "حقن";
                    case "4":
                        return "كريم/مرهم";
                    case "0":
                        return "اخرى";
                    default:
                        return "غير معروف";
                }
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                return "غير معروف";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}