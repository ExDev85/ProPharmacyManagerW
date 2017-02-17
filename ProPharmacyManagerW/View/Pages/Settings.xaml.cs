// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.Kernel;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View.Pages
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

        Config co = new Config();
        IniFile file2 = new IniFile(Paths.BackupConfigPath);

        public static bool IsRecAcc;
        public static bool IsRecMed;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Paths.SetupConfigPath))
            {
                co.Read(true, true);
                DbHost.Text = co.Hostname;
                DbName.Text = co.DbName;
                DbUser.Text = co.DbUserName;
                DbPass.Password = co.DbUserPassword;
                Vers.Content = co.Version;
                if (co.AccountsLog == "0")
                {
                    AccLogs.IsChecked = false;
                }
                if (co.AccountsLog == "1")
                {
                    AccLogs.IsChecked = true;
                }
                if (co.DrugsLog == "0")
                {
                    MedLogs.IsChecked = false;
                }
                if (co.DrugsLog == "1")
                {
                    MedLogs.IsChecked = true;
                }
            }
            if (File.Exists(Paths.BackupConfigPath))
            {
                Core.sb = Core.INIDecrypt(file2.ReadString("Settings", "Backup"));
                Core.st = Core.INIDecrypt(file2.ReadString("Settings", "Type"));
                Core.stt = Core.INIDecrypt(file2.ReadString("Settings", "Time"));
                if (Core.sb == "0")
                {
                    IsBackupActive.IsChecked = false;
                    DailyC.IsChecked = false;
                    WeeklyC.IsChecked = false;
                    MonthlyC.IsChecked = false;
                }
                if (Core.sb == "1")
                {
                    IsBackupActive.IsChecked = true;
                }
                if (Core.st == "0")
                {
                    DailyC.IsChecked = false;
                    WeeklyC.IsChecked = false;
                    MonthlyC.IsChecked = false;
                }
                if (Core.st == "1")
                {
                    DailyC.IsChecked = true;
                    string[] time = Core.stt.Split(':');
                    HourCB.SelectedIndex = Convert.ToByte(time[0]) + 1;
                    MinCB.SelectedIndex = Convert.ToByte(time[1]) + 1;
                }
                if (Core.st == "2")
                {
                    WeeklyC.IsChecked = true;
                    if (Convert.ToByte(Core.stt) == 1)
                    {
                        //monday
                        DayCB.SelectedIndex = 2;
                    }
                    else if (Convert.ToByte(Core.stt) == 2)
                    {
                        //tuesday
                        DayCB.SelectedIndex = 3;
                    }
                    else if (Convert.ToByte(Core.stt) == 3)
                    {
                        //wednesday
                        DayCB.SelectedIndex = 4;
                    }
                    else if (Convert.ToByte(Core.stt) == 4)
                    {
                        //thursday
                        DayCB.SelectedIndex = 5;
                    }
                    else if (Convert.ToByte(Core.stt) == 5)
                    {
                        //friday
                        DayCB.SelectedIndex = 6;
                    }
                    else if (Convert.ToByte(Core.stt) == 6)
                    {
                        //saturday
                        DayCB.SelectedIndex = 0;
                    }
                    else if (Convert.ToByte(Core.stt) == 7)
                    {
                        //sunday
                        DayCB.SelectedIndex = 1;
                    }
                }
                if (Core.st == "3")
                {
                    MonthlyC.IsChecked = true;
                    MonthDayCB.SelectedIndex = Convert.ToByte(Core.stt);

                }
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            CP.BackToMain = true;
        }   
        //database
        private void SetB1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                BackUp.NewDbBackup();
                MessageBox.Show("تم اخذ نسخه احتياطيه من القاعده القديمه");
                DataHolder.CreateConnection(co.DbUserName, co.DbUserPassword, co.Hostname);
                CreateDB.Createdb(co.DbName, DbName.Text);
                co.Write();
                MessageBox.Show("تم الحفظ سيتم غلق البرنامج الان");
                Environment.Exit(0);
            });
        }
        //logs
        private void SetB2_Click(object sender, RoutedEventArgs e)
        {
            if (AccLogs.IsChecked == true)
            {
                co.AccountsLog = "1";
                co.Write(false, false, true, false);
                IsRecAcc = true;
            }
            else
            {
                co.AccountsLog = "0";
                co.Write(false, false, true, false);
                IsRecAcc = false;
            }
            if (MedLogs.IsChecked == true)
            {
                co.DrugsLog = "1";
                co.Write(false, false, false, true);
                IsRecMed = true;
            }
            else
            {
                co.DrugsLog = "0";
                co.Write(false, false, false, true);
                IsRecMed = false;
            }
            co.Read(true, true);
            MessageBox.Show("تم حفظ الاعدادات بنجاح");
        }
        //backups
        private void SetB3_Click(object sender, RoutedEventArgs e)
        {
            if (IsBackupActive.IsChecked == true)
            {
                file2.Write("Settings", "Backup", "1");
            }
            else
            {
                file2.Write("Settings", "Backup", "0");
            }
            if (DailyC.IsChecked == true)
            {
                if (MinCB.Text == "الدقيقة" || HourCB.Text == "الساعة")
                {
                    MessageBox.Show("اختار توقيت صحيح");
                    return;
                }
                else
                {
                    file2.Write("Settings", "Type", "1");
                    file2.Write("Settings", "Time", HourCB.Text + ":" + MinCB.Text);
                    file2.Write("Settings", "Date", DateTime.Now.ToShortDateString());
                }
            }
            else if (WeeklyC.IsChecked == true)
            {
                if (DayCB.Text == "اليوم")
                {
                    MessageBox.Show("اختار يوم من ايام الاسبوع");
                    return;
                }
                else
                {
                    byte dayn;
                    switch (DayCB.Text)
                    {
                        case "الاثنين":
                            dayn = 1;
                            break;
                        case "الثلاثاء":
                            dayn = 2;
                            break;
                        case "الاربعاء":
                            dayn = 3;
                            break;
                        case "الخميس":
                            dayn = 4;
                            break;
                        case "الجمعه":
                            dayn = 5;
                            break;
                        case "السبت":
                            dayn = 6;
                            break;
                        case "الاحد":
                            dayn = 7;
                            break;
                        default:
                            MessageBox.Show("اختار يوم");
                            return;
                    }
                    file2.Write("Settings", "Type", "2");
                    file2.Write("Settings", "Time", dayn);
                }
            }
            else if (MonthlyC.IsChecked == true)
            {
                if (DayCB.Text == "يوم الشهر" || DayCB.Text == "")
                {
                    MessageBox.Show("اختار يوم من ايام الاسبوع");
                    return;
                }
                else
                {
                    file2.Write("Settings", "Type", "3");
                    file2.Write("Settings", "Time", MonthDayCB.Text);
                }
            }
            else
            {
                file2.Write("Settings", "Type", "0");
            }
            MessageBox.Show("تم حفظ الاعدادات بنجاح");
        }
    }
}