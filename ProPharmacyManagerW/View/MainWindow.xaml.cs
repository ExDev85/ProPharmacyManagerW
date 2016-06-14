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

namespace ProPharmacyManagerW.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region window status
        private void Closem1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void imageButton1_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void border1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    //if (this.WindowState == System.Windows.WindowState.Normal)
                    //{
                    //    this.WindowState = System.Windows.WindowState.Maximized;
                    //}
                    //else
                    //{
                    //    this.WindowState = System.Windows.WindowState.Normal;
                    //}
                }
                else
                {
                    this.DragMove();
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (UN.Text != "" || UP.Password != "")
            {
                AccountsTable.UserName = UN.Text;
                AccountsTable.UserPassword = Kernel.Core.GetSHAHashData(UP.Password);
                if (AccountsTable.UserLogin())
                {
                    CP cp = new CP();
                    cp.Show();
                    this.Hide();
                }
                else
                {
                    UP.Focus();
                }
            }
            else
            {
                MessageBox.Show("ادخل اسم المستخدم او كلمه المرور", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                View.About abo = new View.About();
                abo.ShowDialog();
            }
            else if (e.Key == Key.F11)
            {
                Kernel.Core.IsSetup = false;
                Set set = new Set();
                set.Show();
            }
            else if (e.Key == Key.F12)
            {
                if (Kernel.Core.IsCMode == false)
                {
                    View.ConGui consl = new View.ConGui();
                    consl.Show();
                }
            }
            if (UN.Text == "")
            {
                UN.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UN.Focus();
        }

    }
}
