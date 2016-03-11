// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.Kernel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManagerW.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        IniFile file = new IniFile(Constants.SetupConfigPath);

        public static bool IsClosingSet;
        public static bool IsRecAcc;
        public static bool IsRecMed;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdminCP.IsOSettings2 == true)
            {
                tabControl.SelectedIndex = 1;
                AdminCP.IsOSettings2 = false;
            }
            else
            {
                tabControl.SelectedIndex = 0;
                AdminCP.IsOSettings1 = false;
            }
            if (File.Exists(Constants.SetupConfigPath))
            {
                DBHost.Text = Core.INIDecrypt(file.ReadString("MySql", "Host"));
                DBName.Text = Core.INIDecrypt(file.ReadString("MySql", "Database"));
                DBUser.Text = Core.INIDecrypt(file.ReadString("MySql", "Username"));
                DBPass.Password = Core.INIDecrypt(file.ReadString("MySql", "Password"));
                Vers.Content = Core.INIDecrypt(file.ReadString("Upgrade", "Version"));
                if (Core.aa == "0")
                {
                    AccLogs.IsChecked = false;
                }
                if (Core.aa == "1")
                {
                    AccLogs.IsChecked = true;
                }
                if (Core.bb == "0")
                {
                    MedLogs.IsChecked = false;
                }
                if (Core.bb == "1")
                {
                    MedLogs.IsChecked = true;
                }
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            IsClosingSet = true;
        }

        private void SetB2_Click(object sender, RoutedEventArgs e)
        {
            if (AccLogs.IsChecked == true)
            {
                file.Write("Settings", "AccountsLog", "1");
                IsRecAcc = true;
            }
            else
            {
                file.Write("Settings", "AccountsLog", "0");
                IsRecAcc = false;
            }
            if (MedLogs.IsChecked == true)
            {
                file.Write("Settings", "DrugsLog", "1");
                IsRecMed = true;
            }
            else
            {
                file.Write("Settings", "DrugsLog", "0");
                IsRecMed = false;
            }
            Core.aa = Core.INIDecrypt(file.ReadString("Settings", "AccountsLog"));
            Core.bb = Core.INIDecrypt(file.ReadString("Settings", "DrugsLog"));
        }

        private void SetB1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                BackUp.NewDbBackup();
                MessageBox.Show("تم اخذ نسخه احتياطيه من القاعده القديمه");
                DataHolder.CreateConnection(Core.INIDecrypt(file.ReadString("MySql", "Username")), Core.INIDecrypt(file.ReadString("MySql", "Password")), Core.INIDecrypt(file.ReadString("MySql", "Host")));
                CreateDB.Createdb(file.ReadString("MySql", "Database"), DBName.Text);
                file.Write("MySql", "Host", DBHost.Text);
                file.Write("MySql", "Username", DBUser.Text);
                file.Write("MySql", "Password", DBPass.Password);
                file.Write("MySql", "Database", DBName.Text);
                //DataHolder.CreateConnection(Core.INIDecrypt(file.ReadString("MySql", "Username")), Core.INIDecrypt(file.ReadString("MySql", "Password")), Core.INIDecrypt(file.ReadString("MySql", "Database")), Core.INIDecrypt(file.ReadString("MySql", "Host")));
                //BackUp.NewDbRestore();
                //BillsTable.LBN();
                MessageBox.Show("رجاءا اعد تشغيل البرنامج");
                System.Environment.Exit(0);
            });
        }
    }
}
