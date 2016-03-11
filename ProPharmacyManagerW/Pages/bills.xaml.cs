// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Database;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManager.Pages
{
    /// <summary>
    /// Interaction logic for bills.xaml
    /// </summary>
    public partial class Bills : Page
    {
        public Bills()
        {
            InitializeComponent();
        }
        
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            CP.BackToMain = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Order("ID", true);
                    MySqlReader r = new MySqlReader(cmd);
                    while (r.Read())
                    {
                        BillsNoList.Items.Add(r.ReadString("ID"));
                    }
                    r.Close();
                }
                catch (Exception ex)
                {
                    Kernel.Core.SaveException(ex);
                }
            });
        }

        private void BillsNoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BillsNoList.SelectedIndex != -1)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Where("ID", BillsNoList.SelectedItem.ToString());
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read())
                {
                    decimal TP = 0;
                    BillContent.Clear();
                    string AllBills = r.ReadString("Medics");
                    BillContent.Text += "رقم الفاتورة : " + r.ReadString("ID") + "\r\n";
                    BillContent.Text += "الموظف : " + r.ReadString("Cashier") + "\r\n";
                    BillContent.Text += "المشترى : " + r.ReadString("ClientName") + "\r\n";
                    BillContent.Text += "وقت البيع : " + r.ReadString("BillDate") + "\r\n";
                    BillContent.Text += "----الادويه---------------------------------\r\n";
                    string[] bills = AllBills.Split('#');
                    foreach (string[] BillInfo in bills.TakeWhile(bill => bill.Length >= 2).Select(bill => bill.Split('~')))
                    {
                        BillContent.Text += "الاسم : " + Convert.ToString(BillInfo[1]) + "  \t";
                        BillContent.Text += "الكمية : " + Convert.ToString(BillInfo[0]) + " \t";
                        BillContent.Text += "السعر : " + Convert.ToDecimal(BillInfo[2]) + "\r\n";
                        TP += Convert.ToDecimal(BillInfo[2]);
                    }
                    BillContent.Text += "--------------------------------------------\r\n";
                    BillContent.Text += "الاجمالى = " + TP;
                    BillContent.Text += "\r\n--------------------------------------------\r\n";
                }
                r.Close();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ByName.IsChecked == true)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Where("ClientName", SearchBox.Text);
                MySqlReader r = new MySqlReader(cmd);
                BillsNoList.Items.Clear();
                while (r.Read())
                {
                    BillsNoList.Items.Add(r.ReadString("ID"));
                }
                r.Close();
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Where("ID", SearchBox.Text);
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    BillsNoList.Items.Clear();
                    BillsNoList.Items.Add(r.ReadString("ID"));
                }
                r.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BillsNoList.SelectedIndex != -1)
            {
                if (AccountsTable.IsAdmin() == true)
                {
                    try
                    {
                        new MySqlCommand(MySqlCommandType.DELETE).Delete("bills", "ID", BillsNoList.SelectedItem.ToString()).Execute();
                        Console.WriteLine("Delete bill #'" + BillsNoList.SelectedItem.ToString() + "' i hope you don't regret it");
                        MessageBox.Show("تم حذف الفاتورة");
                        BillsNoList.SelectedIndex = -1;
                        BillContent.Text = "لم يتم اختيار اى فواتير";
                    }
                    catch (Exception ex)
                    {
                        Kernel.Core.SaveException(ex);
                    }
                }
            }
            else
            {
                MessageBox.Show("اختار فاتورة اولا ليتم حذفها");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            BillsNoList.Items.Clear();
            Page_Loaded(sender, e);
        }
    }
}
