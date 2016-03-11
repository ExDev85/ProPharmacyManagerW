// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Windows;
using System.Windows.Threading;

namespace ProPharmacyManagerW
{
    /// <summary>
    /// Interaction logic for Accounts.xaml
    /// </summary>
    public partial class Accounts : Window
    {
        public Accounts()
        {
            InitializeComponent();
            
        }
        //sees if it should open the register window or the control panel
        //and where the user come from
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Kernel.Core.IsSetup == true)
            {
                checkRegister.Interval = TimeSpan.FromMilliseconds(500);
                checkRegister.Tick += CheckRegiserState;
                checkRegister.Start();
            }
            if (Pages.AdminCP.edtFromAdm == true)
            {
                Pages.AccCP cu = new Pages.AccCP();
                FAhost.Navigate(cu);
                Pages.AdminCP.edtFromAdm = false;

            }
            else if (Pages.AdminCP.edtStaFromAdm == true)
            {
                Pages.StaCP sc = new Pages.StaCP();
                FAhost.Navigate(sc);
                Pages.AdminCP.edtStaFromAdm = false;
            }
            else if (Pages.AdminCP.LoginLogs == true)
            {
                Pages.AccLogs al = new Pages.AccLogs();
                FAhost.Navigate(al);
                Pages.AdminCP.LoginLogs = false;
            }
        }

        private DispatcherTimer checkRegister = new DispatcherTimer();

        private void CheckRegiserState(object sender, EventArgs e)
        {
            if (Pages.Register.IsRegisterFromSetup == true)
            {
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Pages.Register.IsRegisCom == false && Kernel.Core.IsSetup == true)
            {
                MessageBox.Show("يجب ان تسجل موظف كمدير اولا قبل الاغلاق");
                Console.WriteLine("What?! Are you nuts?!!! Register first you %$#%");
                e.Cancel = true;
            }
            else if (Pages.Register.IsRegisCom == true && Kernel.Core.IsSetup == true)
            {
                e.Cancel = false;
            }
        }
    }
}
