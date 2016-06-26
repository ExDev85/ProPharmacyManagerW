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

namespace ProPharmacyManagerW.View.Pages
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
        
        IniFile file = new IniFile(Paths.SetupConfigPath);

        private BackgroundWorker bgw;
        /// <summary>
        /// Check if installing process is compeleteing or not to load register page
        /// </summary>
        public static bool IsInstallCompleted = false;
        public static bool IsClosing = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\");
            }
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                pB.Height = 10;
            }
            bgw = ((BackgroundWorker)this.FindResource("bgw"));
            try
            {
                if (!File.Exists(Paths.SetupConfigPath))
                {
                    Core.IsSetup = true;
                    Title = "تنصيب البرنامج";
                    pB.Visibility = Visibility.Visible;
                }
                else if (File.Exists(Paths.SetupConfigPath))
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
                Console.WriteLine("Starting to install");
                bgw.RunWorkerAsync();
            }
        }

        private void UpgradeB_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Starting to upgrade");
            file.Write("MySql", "Host", DBHost.Text);
            file.Write("MySql", "Username", DBUser.Text);
            file.Write("MySql", "Password", DBPass.Text);
            file.Write("MySql", "Database", DBName.Text);
            file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            DataHolder.CreateConnection(Core.INIDecrypt(file.ReadString("MySql", "Username")), Core.INIDecrypt(file.ReadString("MySql", "Password")), Core.INIDecrypt(file.ReadString("MySql", "Database")), Core.INIDecrypt(file.ReadString("MySql", "Host")));
            CreateDB.UpgradeTables();
            MessageBox.Show("تمت الترقية بنجاح");
            Console.WriteLine("Upgraded the database you have");
            IsClosing = true;
        }

        private void WConB_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Write the config file");
            file.Write("MySql", "Host", DBHost.Text);
            file.Write("MySql", "Username", DBUser.Text);
            file.Write("MySql", "Password", DBPass.Text);
            file.Write("MySql", "Database", DBName.Text);
            file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            file.Write("Settings", "AccountsLog", "1");
            file.Write("Settings", "DrugsLog", "1");
            MessageBox.Show("تمت كتابه ملف الاعدادت بنجاح");
            Console.WriteLine("I see a little uninstaller in you");
            IsClosing = true;
        }

        private void ExitB_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #region Intalling process
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
            if (!Directory.Exists(Paths.BackupsPath))
            {
                Directory.CreateDirectory(Paths.BackupsPath);
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
            bgw.ReportProgress(95);
            Thread.Sleep(900);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("It's only logical for database to be set");
            MessageBox.Show("تم تنصيب الاعدادات\nمن فضلك انشاء حساب جديد لتتمكن من الدخول");
            pB.Visibility = Visibility.Hidden;
            IsInstallCompleted = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pB.Value = e.ProgressPercentage;
        }
        #endregion

    }
}
