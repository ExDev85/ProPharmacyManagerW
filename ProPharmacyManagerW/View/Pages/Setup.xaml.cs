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
        Config co = new Config();

        private BackgroundWorker bgw;
        /// <summary>
        /// Check if installing process is compeleteing or not to load register page
        /// </summary>
        public static bool IsInstallCompleted = false;
        public static bool IsClosing = false;
        public static bool IsUpgrading = false;

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
                else if (Core.IsUpgrading == true)
                {
                    Core.IsSetup = true;
                    Title = "ترقية البرنامج";
                    pB.Visibility = Visibility.Visible;
                }
                else if (File.Exists(Paths.SetupConfigPath))
                {
                    if (!Core.NoAccount)
                    {
                        Core.IsSetup = false;
                        Title = "اعدادت البرنامج";
                    }
                    Config co = new Config();
                    co.Read();
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
            Config co = new Config
            {
                Hostname = DBHost.Text,
                DbName = DBName.Text,
                DbUserName = DBUser.Text,
                DbUserPassword = DBPass.Text,
            };
            co.Write(true, true, true);
            Console.WriteLine("Config file has been prepared");
            Core.IsUpgrading = true;
            IsUpgrading = true;
        }

        private void ExitB_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #region Intalling process
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bgw.ReportProgress(5);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                co.Hostname = DBHost.Text;
                co.DbName = DBName.Text;
                co.DbUserName = DBUser.Text;
                co.DbUserPassword = DBPass.Text;
                co.AccountsLog = "1";
                co.DrugsLog = "1";
                co.Write(true, true);
            });
            bgw.ReportProgress(30);
            if (!Directory.Exists(Paths.BackupsPath))
            {
                Directory.CreateDirectory(Paths.BackupsPath);
            }
            bgw.ReportProgress(40);
            Thread.Sleep(500);
            DataHolder.CreateConnection(co.DbUserName, co.DbUserPassword, co.Hostname);
            Dispatcher.Invoke((Action)(() =>
            {
                CreateDB.Createdb(co.DbName);
            }));
            Thread.Sleep(500);
            bgw.ReportProgress(60);
            DataHolder.CreateConnection(co.DbUserName, co.DbUserPassword, co.DbName, co.Hostname);
            Dispatcher.Invoke((Action)(CreateDB.CreateTables));
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
