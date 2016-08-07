// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for Employees Control Panel
    /// </summary>
    public partial class UserCP : Page
    {
        public UserCP()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WelMsg.Content = "اهلا بك يا " + AccountsTable.UserName;
        }

        private void Clear()
        {
            MName.Clear();
            MSS.Clear();
            MEX.Text = "";
            MEX.Background = Brushes.White;
            MEX.Foreground = Brushes.Blue;
            MPrice.Clear();
            MExist.Clear();
            MExist.Foreground = Brushes.Blue;
            MExist.Background = Brushes.White;
            MWSell.Text = "1";
            MType.Text = "";
            MNotes.Clear();
        }

        private byte Ptype = 0;
        /// <summary>
        /// Convert drugs type from int to string 
        /// </summary>
        private void MTypeFromToNo()
        {
            switch (Ptype)
            {
                case 1:
                    MType.Text = "شرب";
                    break;
                case 2:
                    MType.Text = "اقراص";
                    break;
                case 3:
                    MType.Text = "حقن";
                    break;
                case 4:
                    MType.Text = "كريم/مرهم";
                    break;
                case 0:
                    MType.Text = "اخرى";
                    break;
                default:
                    MType.Text = "غير معروف";
                    break;
            }
        }

        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ".")
            {
                e.Handled = true;
            }
            if (e.Text == "." && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// check if selling process is complete or not
        /// </summary>
        private bool CompleteSelling = false;
        /// <summary>
        /// Sell command
        /// </summary>
        private void SellMedic()
        {
            //Already existed drugs
            decimal aExist = 0;
            //What is left after selling some
            decimal tot = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("medics").Where("Name", MName.Text).Execute();
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    aExist = r.ReadDecimal("Total");
                }
                if (aExist > 0)
                {
                    tot = aExist - Convert.ToDecimal(MWSell.Text);
                    if (tot >= 0)
                    {
                        MExist.Text = tot.ToString();
                        MySqlCommand CMD = new MySqlCommand(MySqlCommandType.UPDATE);
                        CMD.Update("medics")
                            .Set("Total", tot);
                        CMD.Where("Name", MName.Text).Execute();
                        MessageBox.Show("تم بيع " + MWSell.Text);
                        SaveSold();
                        CompleteSelling = true;
                    }
                    else
                    {
                        MessageBox.Show("الكميه الموجودة لا تناسب ما تحاول بيعه");
                        Console.WriteLine("a little idoit i see in you");
                    }
                }
                else
                {
                    MessageBox.Show("الدواء غير متوفر");
                    Console.WriteLine("So now we sell space");
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }
        /// <summary>
        /// to save selling time
        /// </summary>
        private void SaveSold()
        {
            if (Kernel.Core.bb == "1")
            {
                //calculate the total selling SPrice
                decimal totalpr = Convert.ToDecimal(MPrice.Text) * Convert.ToDecimal(MWSell.Text);
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("medlog")
                        .Insert("MedName", MName.Text)
                        .Insert("TotalAmount", Convert.ToDecimal(MWSell.Text))
                        .Insert("TotalPrice", totalpr)
                        .Insert("SellDate", DateTime.Now.ToString())
                        .Insert("Cashier", AccountsTable.UserName)
                        .Execute();
                    BillsTable.bMAmount = Convert.ToDecimal(MWSell.Text);
                    Console.WriteLine(AccountsTable.UserName + " sold " + Convert.ToDecimal(MWSell.Text) + " from '" + MName.Text + "' for " + totalpr + "he deserve some sweet candy, doesn't he? :)");
                }
                catch (Exception e)
                {
                    Kernel.Core.SaveException(e);
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            try
            {
                if (ByBarCode.IsChecked == true)
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("medics").Where("Barcode", SearchBox.Text).Execute();
                    MySqlReader r = new MySqlReader(cmd);
                    if (r.Read())
                    {
                        MName.Text = r.ReadString("Name");
                        MSS.Text = r.ReadString("ScientificName");
                        Ptype = r.ReadByte("Type");
                        MExist.Text = r.ReadDecimal("Total").ToString();
                        MPrice.Text = r.ReadDecimal("SPrice").ToString();
                        MEX.Text = r.ReadString("ExpirationDate");
                        MNotes.Text = r.ReadString("Notes");
                        SearchBox.Foreground = Brushes.Green;
                        if (Convert.ToDecimal(MExist.Text) < 1)
                        {
                            MExist.Background = Brushes.Red;
                            MExist.Foreground = Brushes.White;
                            Console.WriteLine("You have no - " + MName.Text + " - I believe that you should get new ones");
                        }
                        if (Convert.ToDateTime(MEX.Text) <= DateTime.Now.Date)
                        {
                            MEX.Background = Brushes.Red;
                            MEX.Foreground = Brushes.OrangeRed;
                            Console.WriteLine("Exy exy - " + MName.Text + " - I believe that you should get rid of that");
                        }
                        Console.WriteLine("Searched for - " + MName.Text + " -");
                    }
                    else
                    {
                        SearchBox.Foreground = Brushes.Red;
                        Console.WriteLine("Searched for - " + SearchBox.Text + " - with no luck");
                    }
                }
                else if (ByBarCode.IsChecked == false)
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("medics").Where("Name", SearchBox.Text).Execute();
                    MySqlReader r = new MySqlReader(cmd);
                    if (r.Read())
                    {
                        MName.Text = r.ReadString("Name");
                        MSS.Text = r.ReadString("ScientificName");
                        Ptype = r.ReadByte("Type");
                        MExist.Text = r.ReadDecimal("Total").ToString();
                        MPrice.Text = r.ReadDecimal("SPrice").ToString();
                        MEX.Text = r.ReadString("ExpirationDate");
                        MNotes.Text = r.ReadString("Notes");
                        SearchBox.Foreground = Brushes.Green;
                        if (Convert.ToDecimal(MExist.Text) < 1)
                        {
                            MExist.Background = Brushes.Red;
                            MExist.Foreground = Brushes.White;
                            Console.WriteLine("You have no - " + MName.Text + " - I believe that you should get new ones");
                        }
                        if (Convert.ToDateTime(MEX.Text) <= DateTime.Now.Date)
                        {
                            MEX.Background = Brushes.Red;
                            MEX.Foreground = Brushes.OrangeRed;
                            Console.WriteLine("Exy exy - " + MName.Text + " - I believe that you should get rid of that");
                        }
                        Console.WriteLine("Searched for - " + MName.Text + " -");
                    }
                    else
                    {
                        SearchBox.Foreground = Brushes.Red;
                        Console.WriteLine("Searched for - " + SearchBox.Text + " - with no luck");
                    }
                }
                MTypeFromToNo();
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
            SearchBox.Items.Refresh();
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.Length > 0)
            {
                SearchBox.Foreground = Brushes.Blue;
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!SearchBox.Items.IsEmpty)
            {
                SearchBox.Items.Clear();
            }
            if (SearchBox.IsDropDownOpen == false && SearchBox.Text.Length > 0)
            {
                SearchBox.IsDropDownOpen = true;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("medics").WhereLike("Name", SearchBox.Text);
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read() && SearchBox.Items.Count <= 10 && SearchBox.Text.Length > 0)
                {
                    SearchBox.Items.Add(r.ReadString("Name"));
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        private void SellM_Click(object sender, RoutedEventArgs e)
        {
            if (MName.Text == "" || Client.Text == "" || MPrice.Text == "" || MExist.Text == "" || MWSell.Text == "")
            {
                MessageBox.Show("لا يمكن اتمام عمليه البيع بسبب وجود حقل مهم فارغ");
                return;
            }
            BillsTable.bClient = Client.Text;
            BillsTable.bMName = MName.Text;
            BillsTable.bMCost = Convert.ToDecimal(MPrice.Text) * Convert.ToDecimal(MWSell.Text);
            if (MName.Text != "" && Client.Text != "" && NewBill.IsChecked == true)
            {
                SellMedic();
                if (CompleteSelling != true) return;
                BillsTable.newbill();
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Order("ID");
                    MySqlReader r = new MySqlReader(cmd);
                    if (r.Read())
                    {
                        BillNo.Text = r.ReadString("ID");
                    }
                    r.Close();
                    if (Convert.ToDecimal(MExist.Text) <= 0)
                    {
                        MExist.Background = Brushes.Red;
                        MExist.Foreground = Brushes.White;
                        Console.WriteLine("Searched for - " + MName.Text + " - I believe that you should get new ones");
                    }
                    // to reset it for the new selling process
                    CompleteSelling = false;
                }
                catch (Exception ex)
                {
                    Kernel.Core.SaveException(ex);
                }
            }
            else if (MName.Text != "" && Client.Text != "" && NewBill.IsChecked == false)
            {
                SellMedic();
                if (CompleteSelling == false) return;
                BillsTable.updatebill();
                CompleteSelling = false;
            }
            else if (MName.Text == "" || MPrice.Text == "")
            {
                MessageBox.Show("لا يوجد دواء");
            }
            else if (Client.Text == "")
            {
                MessageBox.Show("ادخل اسم المشترى/العميل");
            }
            else
            {
                MessageBox.Show("الدواء غير متوفر");
            }
        }

        private void UpdateM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("medics")
                    .Set("Notes", MNotes.Text);
                cmd.Where("Name", MName.Text).Execute();
                Console.WriteLine("Update '" + MName.Text + "' Notes I noticed what you did there");
                MessageBox.Show("تم التحديث ملاحظات الدواء");
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            CP.IsLoggingOut = true;
        }

    }
}
