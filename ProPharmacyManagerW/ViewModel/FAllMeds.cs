// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
namespace ProPharmacyManagerW.ViewModel
{
    /// <summary>
    /// ckeck if Checkbox is already checked ;)
    /// </summary>
    public class FAllMeds : Kernel.CommonBase
    {
        private bool _IsByNameChecked = false;
        private bool _IsByBarCodeChecked = false;
        private bool _IsByScientificNameChecked = false;

        public bool IsByNameChecked
        {
            get { return _IsByNameChecked; }
            set
            {
                if (_IsByNameChecked == false)
                {
                    _IsByNameChecked = true;
                    _IsByBarCodeChecked = false;
                    _IsByScientificNameChecked = false;
                    RaisePropertyChanged("IsByNameChecked");
                    RaisePropertyChanged("IsByBarCodeChecked");
                    RaisePropertyChanged("IsByScientificNameChecked");
                }
            }
        }
        public bool IsByBarCodeChecked
        {
            get { return _IsByBarCodeChecked; }
            set
            {
                if (_IsByBarCodeChecked == false)
                {
                    _IsByNameChecked = false;
                    _IsByBarCodeChecked = true;
                    _IsByScientificNameChecked = false;
                    RaisePropertyChanged("IsByNameChecked");
                    RaisePropertyChanged("IsByBarCodeChecked");
                    RaisePropertyChanged("IsByScientificNameChecked");
                }
            }
        }
        public bool IsByScientificNameChecked
        {
            get { return _IsByScientificNameChecked; }
            set
            {
                if (_IsByScientificNameChecked == false)
                {
                    _IsByNameChecked = false;
                    _IsByBarCodeChecked = false;
                    _IsByScientificNameChecked = true;
                    RaisePropertyChanged("IsByNameChecked");
                    RaisePropertyChanged("IsByBarCodeChecked");
                    RaisePropertyChanged("IsByScientificNameChecked");
                }
            }
        }
    }
}