using System.ComponentModel;

namespace ProPharmacyManagerW.Kernel
{
    public class CommonBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Tell the UI about changed property when data binding
        /// </summary>
        /// <param name="propertyName">The property name that has been changed</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }
        #endregion
    }
}
