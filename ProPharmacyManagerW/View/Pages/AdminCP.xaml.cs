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
    /// Interaction logic for Admins Constrol panel
    /// </summary>
    public partial class AdminCP : Page
    {
        public AdminCP()
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
            MType.SelectedIndex = 0;
            MNotes.Clear();
        }

        /// <summary>
        /// Disable Main window control to show panels
        /// </summary>
        private void DisableMain()
        {
            image1.IsEnabled = false;
            menu1.IsEnabled = false;
            groupBox1.IsEnabled = false;
            groupBox2.IsEnabled = false;
            SellM.IsEnabled = false;
            UpdateM.IsEnabled = false;
            DeleteM.IsEnabled = false;
            LogOut.IsEnabled = false;
            ADType.SelectedIndex = 0;
        }

        #region Menuitems
        /// <summary>
        /// Open about window
        /// </summary>
        private void about_Click(object sender, RoutedEventArgs e)
        {
            About abo = new About();
            abo.ShowDialog();
        }
        /// <summary>
        ///add new employee
        /// </summary>
        private void MIAddEMP_Click(object sender, RoutedEventArgs e)
        {
            DisableMain();
            Register re = new Register();
            TwoPanelFame.Navigate(re);
            UserBoard.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// edit password | delete exist employee
        /// </summary>
        private void MIPASEMP_Click(object sender, RoutedEventArgs e)
        {
            AccCP ac = new AccCP();
            TwoPanelFame.Navigate(ac);
            UserBoard.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// edit exist employee state
        /// </summary>
        private void MISTAEMP_Click(object sender, RoutedEventArgs e)
        {
            DisableMain();
            StaCP sc = new StaCP();
            TwoPanelFame.Navigate(sc);
            UserBoard.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// open logs page for login processes
        /// </summary>
        private void MILOGLOG_Click(object sender, RoutedEventArgs e)
        {
            DisableMain();
            AccLogs al = new AccLogs();
            TwoPanelFame.Navigate(al);
            UserBoard.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Database settings page to open
        /// </summary>
        public static bool IsOSettings;
        private void MISet_Click(object sender, RoutedEventArgs e)
        {
            IsOSettings = true;
        }
        /// <summary>
        /// open bills page
        /// </summary>
        public static bool IsBills;
        private void MIBills_Click(object sender, RoutedEventArgs e)
        {
            IsBills = true;
        }
        /// <summary>
        /// open all drugs page
        /// </summary>
        public static bool IsAll;
        private void MIAllMeds_Click(object sender, RoutedEventArgs e)
        {
            IsAll = true;
        }
        /// <summary>
        /// open end expiration page
        /// </summary>
        public static bool IsEX;
        private void MIExy_Click(object sender, RoutedEventArgs e)
        {
            IsEX = true;
        }
        /// <summary>
        /// open end SoldLog page
        /// </summary>
        public static bool IsSL;
        private void MISM_Click(object sender, RoutedEventArgs e)
        {
            IsSL = true;
        }
        /// <summary>
        /// open end SoldLog page
        /// </summary>
        public static bool IsSO;
        private void MISO_Click(object sender, RoutedEventArgs e)
        {
            IsSO = true;
        }
        /// <summary>
        /// open end Backup and restore page
        /// </summary>
        public static bool IsBR;
        private void BAR_Click(object sender, RoutedEventArgs e)
        {
            IsBR = true;
        }
        #endregion

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

        private void SaveSold()
        {
            if (Kernel.Core.bb == "1")
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
                    Console.WriteLine(AccountsTable.UserName + " sold " + Convert.ToDecimal(MWSell.Text) + " from '" + MName.Text + "' for " + totalpr + "he deserve some sweet candy, doesn't he? :)");
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

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            //if (SearchBox.Text.Length == 0)
            //{
            //    SearchBox.Focus();
            //}
        }

        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ".")
            {
                e.Handled = true;
            }
        }

        private void Page_TextInput(object sender, TextCompositionEventArgs e)
        {
            //SearchBox.Focus();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (Kernel.Core.aa == "1")
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("logs").Set("LogoutDate", DateTime.Now.ToString()).Set("Online", 0).Where("Online", 1).Execute();
            }
            CP.IsClosing = true;
        }

        private void UpdateM_Click(object sender, RoutedEventArgs e)
        {
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
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("medics")
                    .Set("Name", MName.Text)
                    .Set("ScientificName", MSS.Text)
                    .Set("ExpirationDate", MEX.Text)
                    .Set("Type", Ptype)
                    .Set("Total", MExist.Text)
                    .Set("SPrice", MPrice.Text)
                    .Set("Notes", MNotes.Text);
                cmd.Where("Name", MName.Text).Execute();
                Console.WriteLine("update the '" + MName.Text + "' drug I hope you are not high");
                MessageBox.Show("تم التحديث");
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        private void DeleteM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new MySqlCommand(MySqlCommandType.DELETE).Delete("medics", "Name", MName.Text).Execute();
                Clear();
                Console.WriteLine("Delete '" + MName.Text + "' now we're talking");
                MessageBox.Show("تم حذف الدواء");
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

        #region Add Drugs Panel
        //back to main menu
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            if (AddNewDrugBoard.Visibility == Visibility.Visible)
            {
                AddNewDrugBoard.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserBoard.Visibility = Visibility.Collapsed;
            }
            image1.IsEnabled = true;
            menu1.IsEnabled = true;
            groupBox1.IsEnabled = true;
            groupBox2.IsEnabled = true;
            SellM.IsEnabled = true;
            UpdateM.IsEnabled = true;
            DeleteM.IsEnabled = true;
            LogOut.IsEnabled = true;
            SearchBox.Focus();
        }
        /// <summary>
        /// Linear Gradient Brush Background
        /// </summary>
        /// <param name="fb">Frist red</param>
        /// <param name="fb">Frist green</param>
        /// <param name="fb">Frist blue</param>
        /// <param name="fb">Second red</param>
        /// <param name="fb">Second green</param>
        /// <param name="fb">Second blue</param>
        void LGBB(byte fr, byte fg, byte fb, byte sr, byte sg, byte sb)
        {
            LinearGradientBrush ng = new LinearGradientBrush();
            ng.StartPoint = new Point(0.5, 0);
            ng.EndPoint = new Point(0.5, 1);
            ng.MappingMode = BrushMappingMode.RelativeToBoundingBox;
            GradientStop gs1 = new GradientStop();
            //#FF3630B4
            gs1.Color = Color.FromRgb(fr, fg, fb);
            gs1.Offset = 0.833;
            ng.GradientStops.Add(gs1);
            GradientStop gs2 = new GradientStop();
            //#FF151083
            gs2.Color = Color.FromRgb(sr, sg, sb);
            gs2.Offset = 1;
            ng.GradientStops.Add(gs2);
            AddNewDrugBoard.Background = ng;
        }
        // add drug panel
        private void MIAddDrug_Click(object sender, RoutedEventArgs e)
        {
            DisableMain();
            AddNewDrugBoard.Visibility = Visibility.Visible;
            ADName.Focus();
            //#3399FF, #0066CC
            LGBB(51, 153, 255, 0, 102, 204);
        }

        private void ADName_KeyDown(object sender, KeyEventArgs e)
        {
            if (ADName.Text == "أسم الدواء*")
            {
                ADName.Text = "";
            }
            if (!ADName.Items.IsEmpty)
            {
                ADName.Items.Clear();
            }
            if (ADName.IsDropDownOpen == false && ADName.Text.Length > 0)
            {
                ADName.IsDropDownOpen = true;
            }
            try
            {
                if (ADName.Text.Length == 0)
                {
                    return;
                }
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("medics").WhereLike("Name", ADName.Text);
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read() && ADName.Items.Count <= 10)
                {
                    ADName.Items.Add(r.ReadString("Name"));
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        private void ADName_GotFocus(object sender, RoutedEventArgs e)
        {
            ADName.Foreground = Brushes.Blue;
            ADName.Background = Brushes.White;
            ADName.Items.Clear();
            //#3399FF, #0066CC
            LGBB(51, 153, 255, 0, 102, 204);
        }

        private void ADName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADName.Text == "")
            {
                ADName.Text = "أسم الدواء*";
            }
        }

        private void RestoreBackground(object sender, RoutedEventArgs e)
        {
            //#3399FF, #0066CC
            LGBB(51, 153, 255, 0, 102, 204);
        }

        private void ADType_GotFocus(object sender, RoutedEventArgs e)
        {
            ADType.Foreground = Brushes.Blue;
            ADType.Background = Brushes.White;
            if (ADType.Text == "النوع*")
            {
                ADType.Text = "";
            }
            //#3399FF, #0066CC
            LGBB(51, 153, 255, 0, 102, 204);
        }

        private void ADType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADType.Text == "")
            {
                ADType.Text = "النوع*";
            }
        }

        // add medic command
        private void AddDrug_Click(object sender, RoutedEventArgs e)
        {
            #region textboxs States
            if (ADName.Text == "أسم الدواء*")
            {
                ADName.Foreground = Brushes.Red;
                return;
            }
            else if (ADName.Text == "")
            {
                ADName.Background = Brushes.Red;
                return;
            }
            if (ADEXP.Text == "تاريخ انتهاء الصلاحيه*")
            {
                ADEXP.Foreground = Brushes.Red;
                return;
            }
            else if (ADEXP.Text == "")
            {
                MessageBox.Show("ادخل تاريخ انتهاء الصلاحية");
                return;
            }
            else if (ADEXP.SelectedDate <= DateTime.Now.Date)
            {
                ADEXP.Background = Brushes.Red;
                return;
            }
            if (ADPrice.Text == "")
            {
                ADPrice.Background = Brushes.Red;
                return;
            }
            else if (Convert.ToDecimal(ADPrice.Text) <= 0)
            {
                ADPrice.Foreground = Brushes.Red;
                return;
            }
            if (ADTotal.Text == "")
            {
                ADTotal.Background = Brushes.Red;
                return;
            }
            else if (Convert.ToDecimal(ADTotal.Text) <= 0)
            {
                ADTotal.Foreground = Brushes.Red;
                return;
            }
            #endregion
            byte PType;
            switch (ADType.Text)
            {
                case "شرب":
                    PType = 1;
                    break;
                case "اقراص":
                    PType = 2;
                    break;
                case "حقن":
                    PType = 3;
                    break;
                case "كريم/مرهم":
                    PType = 4;
                    break;
                case "اخرى":
                    PType = 0;
                    break;
                default:
                    ADType.Background = Brushes.Red;
                    return;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("medics")
                    .Insert("Name", ADName.Text)
                    .Insert("Barcode", ADBarCode.Text)
                    .Insert("ScientificName", ADSS.Text)
                    .Insert("ExpirationDate", ADEXP.Text)
                    .Insert("Type", PType)
                    .Insert("Total", Convert.ToDecimal(ADTotal.Text))
                    .Insert("SPrice", Convert.ToDecimal(ADPrice.Text))
                    .Insert("Notes", ADNote.Text).Execute();
                //#2ECC71, #2aba66
                LGBB(46, 204, 113, 42, 186, 102);
                Console.WriteLine(AccountsTable.UserName + " add " + ADTotal.Text + " " + ADName.Text + " which each cost " + ADPrice.Text);
            }
            catch (Exception ex)
            {
                //#d8334a, #BF263C
                LGBB(216, 51, 74, 191, 38, 60);
                MessageBox.Show("غالبا تم استخدام نفس اسم الدواء من قبل");
                Kernel.Core.SaveException(ex);
            }
        }
        #endregion
    }
}