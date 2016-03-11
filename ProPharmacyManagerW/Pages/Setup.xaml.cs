// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.Kernel;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManagerW.Pages
{
    /// <summary>
    /// setup program page
    /// </summary>
    public partial class Setup : Page
    {
        public Setup()
        {
            InitializeComponent();
        }

        IniFile file = new IniFile(Constants.SetupConfigPath);

        private BackgroundWorker bgw;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                pB.Height = 10;
            }
            bgw = ((BackgroundWorker)this.FindResource("bgw"));
            try
            {
                if (!File.Exists(Constants.SetupConfigPath))
                {
                    Core.IsSetup = true;
                    Title = "تنصيب البرنامج";
                    pB.Visibility = Visibility.Visible;
                }
                else if (File.Exists(Constants.SetupConfigPath))
                {
                    Core.IsSetup = false;
                    Title = "اعدادت البرنامج";
                    DBHost.Text = Core.INIDecrypt(file.ReadString("MySql", "Host"));
                    DBName.Text = Core.INIDecrypt(file.ReadString("MySql", "Database"));
                    DBUser.Text = Core.INIDecrypt(file.ReadString("MySql", "Username"));
                    DBPass.Text = Core.INIDecrypt(file.ReadString("MySql", "Password"));
                }

            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
        }   

        /// <summary>
        /// setup button
        /// </summary>
        /// 
        private void SetB_Click(object sender, RoutedEventArgs e)
        {
            if (Core.IsSetup == true)
            {
                bgw.RunWorkerAsync();
            }
        }

        private void UpgradeB_Click(object sender, RoutedEventArgs e)
        {
            file.Write("MySql", "Host", DBHost.Text);
            file.Write("MySql", "Username", DBUser.Text);
            file.Write("MySql", "Password", DBPass.Text);
            file.Write("MySql", "Database", DBName.Text);
            file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            DataHolder.CreateConnection(Core.INIDecrypt(file.ReadString("MySql", "Username")), Core.INIDecrypt(file.ReadString("MySql", "Password")), Core.INIDecrypt(file.ReadString("MySql", "Database")), Core.INIDecrypt(file.ReadString("MySql", "Host")));
            CreateDB.UpgradeTables();
            MessageBox.Show("تمت الترقية بنجاح");
            Console.WriteLine("Upgraded the database you have");
            if (Core.IsSetup == true)
            {
                Register.IsRegisterFromSetup = true;
            }
        }

        private void WConB_Click(object sender, RoutedEventArgs e)
        {
            file.Write("MySql", "Host", DBHost.Text);
            file.Write("MySql", "Username", DBUser.Text);
            file.Write("MySql", "Password", DBPass.Text);
            file.Write("MySql", "Database", DBName.Text);
            file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            file.Write("Settings", "AccountsLog", "1");
            file.Write("Settings", "DrugsLog", "1");
            MessageBox.Show("تمت كتابه ملف الاعدادت بنجاح");
            Console.WriteLine("I see a little uninstaller in you");
            if (Core.IsSetup == true)
            {
                Register.IsRegisterFromSetup = true;
            }
        }

        private void ExitB_Click(object sender, RoutedEventArgs e)
        {
            if (Core.IsSetup == true)
            {
                Environment.Exit(0);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bgw.ReportProgress(5);
            Thread.Sleep(500);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                file.Write("MySql", "Host", DBHost.Text);
                file.Write("MySql", "Username", DBUser.Text);
                file.Write("MySql", "Password", DBPass.Text);
                file.Write("MySql", "Database", DBName.Text);
                file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                file.Write("Settings", "AccountsLog", "1");
                file.Write("Settings", "DrugsLog", "1");
            });
            bgw.ReportProgress(30);
            if (!Directory.Exists("BackUp"))
            {
                Directory.CreateDirectory("BackUp");
            }
            Thread.Sleep(500);

            bgw.ReportProgress(40);
            Thread.Sleep(500);
            DataHolder.CreateConnection(Core.INIDecrypt(file.ReadString("MySql", "Username")), Core.INIDecrypt(file.ReadString("MySql", "Password")), Core.INIDecrypt(file.ReadString("MySql", "Host")));
            Dispatcher.Invoke((Action)(() =>
            {
                CreateDB.Createdb(DBName.Text);

            }));
            Thread.Sleep(500);
            bgw.ReportProgress(60);
            DataHolder.CreateConnection(Core.INIDecrypt(file.ReadString("MySql", "Username")), Core.INIDecrypt(file.ReadString("MySql", "Password")), Core.INIDecrypt(file.ReadString("MySql", "Database")), Core.INIDecrypt(file.ReadString("MySql", "Host")));
            Dispatcher.Invoke((Action)(() =>
            {
                CreateDB.CreateTables();
            }));
            bgw.ReportProgress(100);
            Thread.Sleep(900);

        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("It's only logical for database to be set");
            MessageBox.Show("تم تنصيب الاعدادات\nمن فضلك انشاء حساب جديد لتتمكن من الدخول");
            pB.Visibility = Visibility.Hidden;
            Accounts reg = new Accounts();
            reg.Title = "اضافه مستخدم";
            reg.ShowDialog();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pB.Value = e.ProgressPercentage;
        }

        
    }
}
