// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.Kernel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for Admins Constrol panel
    /// </summary>
    public partial class MainCP : Page
    {
        public MainCP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// reset textboxs every new search to default
        /// </summary>
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
            MType.SelectedIndex = -1;
            MNotes.Clear();
        }
        /// <summary>
        /// Save item id for updating deleting stuff
        /// </summary>
        private Int64 ItemId = 0;

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
        /// <summary>
        /// check if selling process is complete or not
        /// </summary>
        private bool CompleteSelling = false;

        /// <summary>
        /// Selling command
        /// </summary>
        private void SellMedic()
        {
            if (Convert.ToDateTime(MEX.Text) <= DateTime.Now.Date)
            {
                MessageBoxResult result = MessageBox.Show("الدواء منتهى الصلاحيه\nهل تريد الاستمرار", "تحذير", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        Console.WriteLine("WeWa WeWa WeWa WeWa\n\r this is the police sirens run .. run");
                        MessageBox.Show("جارى استكمال عمليه البيع");
                        break;
                    case MessageBoxResult.Cancel:
                        Console.WriteLine("Goodboy");
                        MessageBox.Show("تم ايقاف عمليه البيع");
                        return;
                }
            }
            //Already existed drugs
            decimal aExist = 0;
            //What is left after selling some
            decimal tot = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("medics").Where("Id", ItemId).And("Name", MName.Text).Execute();
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
                            .Set("Total", tot).Where("Name", MName.Text).Execute();
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
                    Console.WriteLine("So basically you are tring to copy Apple business model");
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        /// <summary>
        /// Makes logs for sold meds
        /// </summary>
        private void SaveSold()
        {
            Config co = new Config();
            if (co.DrugsLog == "1")
            {
                //calculate the total selling price
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
                    Console.WriteLine(AccountsTable.UserName + " sold " + Convert.ToDecimal(MWSell.Text) + " from '" + MName.Text + " - " + ItemId + "' for " + totalpr + "he deserve some sweet candy, doesn't he? :)");
                }
                catch (Exception e)
                {
                    Kernel.Core.SaveException(e);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WelMsg.Content = "اهلا بك يا " + AccountsTable.UserName;
            BillNo.Text = BillsTable.BillNO.ToString();
            SearchBox.Focus();
            if (AccountsTable.IsAdmin() == false)
            {
                MName.IsReadOnly = true;
                MSS.IsReadOnly = true;
                MSUP.IsEditable = false;
                MSUP.IsReadOnly = true;
                MType.IsEditable = false;
                MType.IsReadOnly = true;
                MExist.IsReadOnly = true;
                MPrice.IsReadOnly = true;
            }
            //custom UI changes for XP
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                Client.FontSize = 10;
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
                        ItemId = r.ReadInt64("Id");
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
                            Console.WriteLine("You have no - " + MName.Text + " - " + ItemId + " - I believe that you should get new ones");
                        }
                        if (Convert.ToDateTime(MEX.Text) <= DateTime.Now.Date)
                        {
                            MEX.Background = Brushes.Red;
                            MEX.Foreground = Brushes.OrangeRed;
                            Console.WriteLine("bad deadpool - " + MName.Text + " - " + ItemId + " - you should get rid of that");
                        }
                        Console.WriteLine("Searched for - " + MName.Text + " - " + ItemId + " -");
                    }
                    else
                    {
                        SearchBox.Foreground = Brushes.Red;
                        Console.WriteLine("Searched for - " + SearchBox.Text + " - with no luck");
                    }
                    r.Close();
                    ItemsList.Items.Clear();
                    while (r.Read())
                    {
                        ItemsList.Items.Add(r.ReadInt64("Id"));
                    }
                    if (ItemsList.Items.Count < 1)
                    {
                        ItemsList.Items.Clear();
                        ItemsList.Items.Add("لا يوجد شئ اخر");
                    }
                    r.Close();
                }
                else if (ByBarCode.IsChecked == false)
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("medics").Where("Name", SearchBox.Text).Execute();
                    MySqlReader r = new MySqlReader(cmd);
                    if (r.Read())
                    {
                        ItemId = r.ReadInt64("Id");
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
                            Console.WriteLine("Red Alert - " + MName.Text + " - you should get rid of that");
                        }
                        Console.WriteLine("Searched for - " + MName.Text + " -");
                    }
                    else
                    {
                        SearchBox.Foreground = Brushes.Red;
                        Console.WriteLine("Searched for - " + SearchBox.Text + " - with no luck");
                    }
                    r.Close();
                    ItemsList.Items.Clear();
                    while (r.Read())
                    {
                        ItemsList.Items.Add(r.ReadInt64("Id"));
                    }
                    if (ItemsList.Items.Count < 1)
                    {
                        ItemsList.Items.Clear();
                        ItemsList.Items.Add("لا يوجد شئ اخر");
                    }
                    r.Close();
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
            if (SearchBox.IsDropDownOpen == false)
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

        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ".")
            {
                e.Handled = true;
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            CP.IsLoggingOut = true;
        }

        private void UpdateM_Click(object sender, RoutedEventArgs e)
        {
            if (MName.Text == "" || MPrice.Text == "" || MExist.Text == "" || MEX.Text == "")
            {
                MessageBox.Show("لا يمكن اتمام عمليه التحديث بسبب وجود حقل مهم فارغ");
                return;
            }
            try
            {
                switch (MType.Text)
                {
                    case "شرب":
                        Ptype = 1;
                        break;
                    case "اقراص":
                        Ptype = 2;
                        break;
                    case "حقن":
                        Ptype = 3;
                        break;
                    case "كريم/مرهم":
                        Ptype = 4;
                        break;
                    case "اخرى":
                        Ptype = 0;
                        break;
                }
                if (AccountsTable.IsAdmin() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmd.Update("medics")
                        .Set("Name", MName.Text)
                        .Set("ScientificName", MSS.Text)
                        .Set("ExpirationDate", MEX.Text)
                        .Set("Type", Ptype)
                        .Set("Total", MExist.Text)
                        .Set("SPrice", MPrice.Text)
                        .Set("Notes", MNotes.Text);
                    cmd.Where("Id", ItemId).And("Name", MName.Text).Execute();
                    Console.WriteLine("update the '" + MName.Text + " - " + ItemId.ToString() + "' drug I hope you are not high");
                    MessageBox.Show("تم التحديث");
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmd.Update("medics")
                        .Set("Notes", MNotes.Text);
                    cmd.Where("Name", MName.Text).Execute();
                    Console.WriteLine("Update '" + MName.Text + " - " + ItemId.ToString() + "' Notes, I noticed what you did there");
                    MessageBox.Show("تم التحديث ملاحظات الدواء");
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
        }

        private void DeleteM_Click(object sender, RoutedEventArgs e)
        {
            if (MName.Text == "" || MPrice.Text == "" || MExist.Text == "")
            {
                MessageBox.Show("لا يمكن اتمام عمليه الحذف");
                return;
            }
            try
            {
                if (AccountsTable.IsAdmin() == true)
                {
                    new MySqlCommand(MySqlCommandType.DELETE).Delete("medics", "id", ItemId).Execute();
                    Clear();
                    Console.WriteLine("Delete '" + MName.Text + " - " + ItemId.ToString() + "' now we're talking");
                    MessageBox.Show("تم حذف الدواء");
                }
                else
                {
                    Console.WriteLine(AccountsTable.UserName + "was trying to Delete '" + MName.Text + " - " + ItemId.ToString());
                    MessageBox.Show("يجب ان تكون مدير لتستطيع الحذف");
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
        }

        private void SellM_Click(object sender, RoutedEventArgs e)
        {
            if (MName.Text == "" || MPrice.Text == "" || MExist.Text == "" || MWSell.Text == "")
            {
                MessageBox.Show("لا يمكن اتمام عمليه البيع بسبب وجود حقل مهم فارغ");
                return;
            }
            if (EnBills.IsChecked == true)
            {
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
                        CompleteSelling = false;
                    }
                    catch (Exception ex)
                    {
                        Core.SaveException(ex);
                    }
                }
                else if (MName.Text != "" && Client.Text != "" && NewBill.IsChecked == false)
                {
                    SellMedic();
                    if (CompleteSelling == false) return;
                    BillsTable.updatebill();
                    CompleteSelling = false;
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
            else
            {
                SellMedic();
                CompleteSelling = false;
            }
        }

        private void ItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("medics").Where("Id", ItemsList.SelectedItem.ToString()).And("Name", SearchBox.Text).Execute();
            MySqlReader r = new MySqlReader(cmd);
            Clear();
            if (r.Read())
            {
                ItemId = r.ReadInt64("Id");
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
                MTypeFromToNo();
                Console.WriteLine("Searched for - " + MName.Text + " -");
            }
        }

    }
}