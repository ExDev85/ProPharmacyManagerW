// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Kernel;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ProPharmacyManager
{
    /// <summary>
    /// Interaction logic for Set.xaml
    /// which has pages for setup, upgrade and the program settings
    /// </summary>
    public partial class Set : Window
    {
        public Set()
        {
            InitializeComponent();           
        }

        //Timer to close the config window after finish setup the porgram for the first time
        private DispatcherTimer checkSetUp = new DispatcherTimer();

        private void checkSetUpState(object sender, EventArgs e)
        {
            if (Pages.Register.IsRegister == true && Core.IsSetup == true)
            {
                this.Close();
                Core.IsSetup = false;
                Pages.Register.IsRegister = false;
                checkSetUp.Stop();
            }
        }
        /// <summary>
        /// This code check if the form should load setup or settings page
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Core.IsSetup == true)
            {
                checkSetUp.Interval = TimeSpan.FromMilliseconds(500);
                checkSetUp.Tick += checkSetUpState;
                checkSetUp.Start();
            }

        }
    }
}
