// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
namespace ProPharmacyManagerW.ViewModel
{
    public class ABackup : Kernel.CommonBase
    {
        private bool _IsDailyChecked = false;
        private bool _IsWeeklyChecked = false;
        private bool _IsMonthlyChecked = false;
        
        public bool IsDailyChecked
        {
            get { return _IsDailyChecked; }
            set
            {
                if (_IsDailyChecked == false)
                {
                    _IsDailyChecked = true;
                    _IsWeeklyChecked = false;
                    _IsMonthlyChecked = false;
                    RaisePropertyChanged("IsWeeklyChecked");
                    RaisePropertyChanged("IsMonthlyChecked");
                    RaisePropertyChanged("IsDailyChecked");
                }
                else
                {
                    _IsDailyChecked = false;
                    RaisePropertyChanged("IsDailyChecked");
                }
            }
        }
        public bool IsWeeklyChecked
        {
            get { return _IsWeeklyChecked; }
            set
            {
                if (_IsWeeklyChecked == false)
                {
                    _IsDailyChecked = false;
                    _IsWeeklyChecked = true;
                    _IsMonthlyChecked = false;
                    RaisePropertyChanged("IsDailyChecked");
                    RaisePropertyChanged("IsMonthlyChecked");
                    RaisePropertyChanged("IsWeeklyChecked");
                }
                else
                {
                    _IsWeeklyChecked = false;
                    RaisePropertyChanged("IsWeeklyChecked");
                }
            }
        }
        public bool IsMonthlyChecked
        {
            get { return _IsMonthlyChecked; }
            set
            {
                if (_IsMonthlyChecked == false)
                {
                    _IsDailyChecked = false;
                    _IsWeeklyChecked = false;
                    _IsMonthlyChecked = true;
                    RaisePropertyChanged("IsDailyChecked");
                    RaisePropertyChanged("IsWeeklyChecked");
                    RaisePropertyChanged("IsMonthlyChecked");
                }
                else
                {
                    _IsMonthlyChecked = false;
                    RaisePropertyChanged("IsMonthlyChecked");
                }
            }
        }
    }
}
