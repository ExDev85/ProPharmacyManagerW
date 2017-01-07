// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Kernel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View
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
            if (Pages.Setup.IsClosing == true)
            {
                Pages.Setup.IsClosing = false;
                Close();
                checkSetUp.Stop();
            }
            if (Core.IsSetup == true)
            {
                if (Pages.Setup.IsInstallCompleted)
                {
                    Pages.Setup.IsInstallCompleted = false;
                    Pages.Register re = new Pages.Register();
                    setreg.Navigate(re);
                }
                if (Pages.Register.IsRegisCom)
                {
                    Pages.Register.IsRegisCom = false;
                    Core.IsSetup = false;
                    Close();
                    checkSetUp.Stop();
                }
                if (Pages.Setup.IsUpgrading)
                {
                    Pages.Upgrade ug = new Pages.Upgrade();
                    setreg.Navigate(ug);
                    Pages.Setup.IsUpgrading = false;
                }
                else if (Pages.Upgrade.IsUpgradeComp)
                {
                    Core.IsSetup = false;
                    Pages.Setup.IsUpgrading = false;
                    Pages.Upgrade.IsUpgradeComp = false;
                    Close();
                    checkSetUp.Stop();
                }
            }
        }

        /// <summary>
        /// This code check if the form should load setup or settings page
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //#FF2B41A4
            SolidColorBrush NC = new SolidColorBrush {Color = Color.FromRgb(43, 65, 164)};
            this.Background = NC;
            if (Core.IsUpgrading == true)
            {
                Pages.Setup.IsUpgrading = true;
            }
            checkSetUp.Interval = TimeSpan.FromMilliseconds(500);
            checkSetUp.Tick += checkSetUpState;
            checkSetUp.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                About abo = new About();
                abo.ShowDialog();
            }
            else if (e.Key == Key.F12)
            {
                if (Core.IsCMode == false)
                {
                    ConGui consl = new ConGui();
                    consl.Show();
                }
            }
        }

    }
}
