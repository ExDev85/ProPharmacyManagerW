// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Kernel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProPharmacyManagerW
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
            if (Pages.Register.IsRegisterFromSetup == true && Core.IsSetup == true)
            {
                this.Close();
                Core.IsSetup = false;
                Pages.Register.IsRegisterFromSetup = false;
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
                //#FF2B41A4
                SolidColorBrush NC = new SolidColorBrush();
                NC.Color = Color.FromRgb(43, 65, 164);
                this.Background = NC;
                checkSetUp.Interval = TimeSpan.FromMilliseconds(500);
                checkSetUp.Tick += checkSetUpState;
                checkSetUp.Start();
            }

        }
    }
}
