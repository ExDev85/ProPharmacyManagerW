// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View
{
    /// <summary>
    /// Interaction logic for CP.xaml
    /// </summary>
    public partial class CP : Window
    {
        public CP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if user want to get back to main cp
        /// </summary>
        public static bool BackToMain;
        /// <summary>
        /// If a page is closing or not
        /// </summary>
        public static bool IsClosing;

        private DispatcherTimer checkClosing = new DispatcherTimer();

        #region Window Status
        //The title bar to maxmize the form when user double click it or drag it by hold
        private void border1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    if (this.WindowState == WindowState.Normal)
                    {
                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                    }
                }
                else
                {
                    this.DragMove();
                }
            }
        }
        //X Button that close the current form
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //to record the logout date 
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("logs").Set("LogoutDate", DateTime.Now.ToString()).Set("Online", 0).Where("Online", 1).Execute();
            //open login window after user logs out
            MainWindow loginw = new MainWindow();
            loginw.Show();
            this.Close();
        }
        // - Button that minimize the current form
        private void imageButton1_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        private void PCP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                View.About abo = new View.About();
                abo.ShowDialog();
            }
            else if (e.Key == Key.F12)
            {
                if (Kernel.Core.IsCMode == false)
                {
                    View.ConGui consl = new View.ConGui();
                    consl.Show();
                }
            }
        }

        private void PCP_Loaded(object sender, RoutedEventArgs e)
        {
            if (AccountsTable.IsAdmin() == true)
            {
                this.Title = "لوحه المدراء";
            }
            else if (AccountsTable.IsAdmin() == false)
            {
                this.Title = "لوحه الموظفين";
                Pages.UserCP cu = new Pages.UserCP();
                FFhost.Navigate(cu);
            }
            checkClosing.Interval = TimeSpan.FromMilliseconds(500);
            checkClosing.Tick += checkClosingState;
            checkClosing.Start();
        }

        private void checkClosingState(object sender, EventArgs e)
        {
            if (Pages.AdminCP.IsOSettings)
            {
                Pages.Settings set = new Pages.Settings();
                FFhost.Navigate(set);
                Pages.AdminCP.IsOSettings = false;
            }
            if (Pages.Settings.IsClosingSet && AccountsTable.IsAdmin() == false)
            {
                this.Title = "لوحه الموظفين";
                Pages.UserCP cu = new Pages.UserCP();
                FFhost.Navigate(cu);
                Pages.Settings.IsClosingSet = false;
            }
            else if (Pages.Settings.IsClosingSet && AccountsTable.IsAdmin() == true)
            {
                this.Title = "لوحه المدراء";
                Pages.AdminCP ac = new Pages.AdminCP();
                FFhost.Navigate(ac);
                Pages.Settings.IsClosingSet = false;
            }
            if (IsClosing == true)
            {
                checkClosing.Stop();
                MainWindow loginw = new MainWindow();
                IsClosing = false;
                loginw.Show();
                this.Close();
            }
            if (BackToMain == true)
            {
                if (AccountsTable.IsAdmin() == true)
                {
                    Pages.AdminCP ac = new Pages.AdminCP();
                    FFhost.Navigate(ac);
                }
                else
                {
                    Pages.UserCP cu = new Pages.UserCP();
                    FFhost.Navigate(cu);
                }
                BackToMain = false;
            }
            if (Pages.AdminCP.IsBills == true)
            {
                Pages.Bills bl = new Pages.Bills();
                FFhost.Navigate(bl);
                Pages.AdminCP.IsBills = false;
            }
            else if (Pages.AdminCP.IsAll == true)
            {
                Pages.AllMeds am = new Pages.AllMeds();
                FFhost.Navigate(am);
                Pages.AdminCP.IsAll = false;
            }
            else if (Pages.AdminCP.IsEX == true)
            {
                Pages.Expiration ex = new Pages.Expiration();
                FFhost.Navigate(ex);
                Pages.AdminCP.IsEX = false;
            }
            else if (Pages.AdminCP.IsSL == true)
            {
                Pages.SoldLogs sl = new Pages.SoldLogs();
                FFhost.Navigate(sl);
                Pages.AdminCP.IsSL = false;
            }
            else if (Pages.AdminCP.IsSO == true)
            {
                Pages.Out so = new Pages.Out();
                FFhost.Navigate(so);
                Pages.AdminCP.IsSO = false;
            }
            else if (Pages.AdminCP.IsBR == true)
            {
                Pages.BAR br = new Pages.BAR();
                FFhost.Navigate(br);
                Pages.AdminCP.IsBR = false;
            }
        }
    }
}
