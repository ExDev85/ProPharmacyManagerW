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

        /// <summary>
        /// Check if installing process is compeleted or not to load register page
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
                PB.Height = 10;
            }
            try
            {
                if (!File.Exists(Paths.SetupConfigPath))
                {
                    Core.IsSetup = true;
                    Title = "تنصيب البرنامج";
                    PB.Visibility = Visibility.Visible;
                }
                else if (Core.IsUpgrading == true)
                {
                    Core.IsSetup = true;
                    Title = "ترقية البرنامج";
                    PB.Visibility = Visibility.Visible;
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
        private void SetB_Click(object sender, RoutedEventArgs e)
        {
            if (Core.IsSetup)
            {
                Console.WriteLine("Starting to install");
                BackgroundWorker bgw = new BackgroundWorker()
                {
                    WorkerReportsProgress = true
                };
                bgw.DoWork += BackgroundWorker_DoWork;
                bgw.ProgressChanged += BackgroundWorker_ProgressChanged;
                bgw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                bgw.RunWorkerAsync();
                SetB.IsEnabled = false;
                UpgradeB.IsEnabled = false;
                DbHost.IsEnabled = false;
                DbName.IsEnabled = false;
                DbUser.IsEnabled = false;
                DbPass.IsEnabled = false;
            }
        }

        private void UpgradeB_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Starting to upgrade");
            Config co = new Config
            {
                Hostname = DbHost.Text,
                DbName = DbName.Text,
                DbUserName = DbUser.Text,
                DbUserPassword = DbPass.Text,
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
            (sender as BackgroundWorker)?.ReportProgress(5);
            Config co = new Config();           
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart) delegate()
            {
                co.Hostname = DbHost.Text;
                co.DbName = DbName.Text;
                co.DbUserName = DbUser.Text;
                co.DbUserPassword = DbPass.Text;
                co.AccountsLog = "1";
                co.DrugsLog = "1";
                co.Write(true, true);
            });
            (sender as BackgroundWorker)?.ReportProgress(30);
            if (!Directory.Exists(Paths.BackupsPath))
            {
                Directory.CreateDirectory(Paths.BackupsPath);
            }
            (sender as BackgroundWorker)?.ReportProgress(40);
            Dispatcher.Invoke((Action) (() =>
            {
                DataHolder.CreateConnection(co.DbUserName, co.DbUserPassword, co.Hostname);
                CreateDB.Createdb(co.DbName);
                DataHolder.CreateConnection(co.DbUserName, co.DbUserPassword, co.DbName, co.Hostname);
            }));
            (sender as BackgroundWorker)?.ReportProgress(60);
            Dispatcher.Invoke((Action)(CreateDB.CreateTables));
            (sender as BackgroundWorker)?.ReportProgress(95);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("It's only logical for database to be set");
            MessageBox.Show("تم تنصيب الاعدادات\nمن فضلك انشاء حساب جديد لتتمكن من الدخول");
            PB.Visibility = Visibility.Hidden;
            IsInstallCompleted = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PB.Value = e.ProgressPercentage;
        }
        #endregion

    }
}
