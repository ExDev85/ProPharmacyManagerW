// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Database;
using ProPharmacyManager.Kernel;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ProPharmacyManager.Pages
{
    /// <summary>
    /// Interaction logic for BAR.xaml
    /// </summary>
    public partial class BAR : Page
    {
        public BAR()
        {
            InitializeComponent();
        }

        private void Reload()
        {
            BackUpList.Items.Clear();
            DirectoryInfo dinfo = new DirectoryInfo(PathT.Text);
            FileInfo[] Files = dinfo.GetFiles("*.sql");
            foreach (FileInfo file in Files)
            {
                BackUpList.Items.Add(file.Name);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IniFile file = new IniFile(Constants.BackupConfigPath);
            if (File.Exists(Constants.BackupConfigPath))
            {
                PathT.Text = file.ReadString("BackUp", "Path");
            }
            else if (!File.Exists(Constants.BackupConfigPath))
            {
                DirectoryInfo dir1 = new DirectoryInfo("BackUp\\");
                string[] lines = { "[BackUp]", "Path=" + dir1.FullName };
                File.WriteAllLines(Constants.BackupConfigPath, lines);
                PathT.Text = file.ReadString("BackUp", "Path");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"BackUp\"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\BackUp\");
            }
            DirectoryInfo dinfo = new DirectoryInfo(PathT.Text);
            FileInfo[] Files = dinfo.GetFiles("*.sql");
            short CountBacks = 0;
            foreach (FileInfo file1 in Files)
            {
                BackUpList.Items.Add(file1.Name);
                CountBacks++;
            }
            Count.Content = "عدد النسخ الاحتياطية : " + CountBacks;
        }

        private void BackUpB_Click(object sender, RoutedEventArgs e)
        {
            if (ENKT.Text != "")
            {
                BackUp.Backup(ENKT.Text);
            }
            else
            {
                MessageBox.Show("اختر مفتاح تشفير بالانجلزيه و احفظه جيدا، ستحتاجه فى كل مرة تسترجع النسخة الإحتياطيه");
            }
            Reload();
        }

        private void RestoreB_Click(object sender, RoutedEventArgs e)
        {
            if (ENKT.Text != "")
            {
                if (BackUpList.SelectedIndex != -1)
                {
                    BackUp.Restore(PathT.Text + BackUpList.SelectedItem, ENKT.Text);
                }
                else
                {
                    MessageBox.Show("اختر نسخه اولا ليتم استعادتها ");
                }
            }
            else
            {
                MessageBox.Show("ادخل مفتاح التشفير الذى اخترته للملف من قبل اولا");
            }
        }

        private void DelB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(PathT.Text + BackUpList.SelectedItem);
                Reload();
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                MessageBox.Show("اختار ملف اولا");
            }
        }

        private void BackB_Click(object sender, RoutedEventArgs e)
        {
            CP.BackToMain = true;
        }

        private void BrowserB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(PathT.Text);
            }
            catch
            {
                MessageBox.Show("تاكد من صحه المسار");
            }
        }
    }
}
