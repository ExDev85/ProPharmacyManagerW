﻿// <copyright>
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

namespace ProPharmacyManagerW.Pages
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

        #region Menuitems
        /// <summary>
        /// Open about window
        /// </summary>
        private void about_Click(object sender, RoutedEventArgs e)
        {
            Forms.About abo = new Forms.About();
            abo.ShowDialog();
        }
        /// <summary>
        /// Change password
        /// </summary>
        private void MIPASEMP_Click(object sender, RoutedEventArgs e)
        {
            AdminCP.edtFromAdm = true;
            Accounts acc = new Accounts();
            acc.Title = "لوحه التحكم";
            acc.Show();
        }
        /// <summary>
        /// edit exist employee state
        /// </summary>
        private void MISTAEMP_Click(object sender, RoutedEventArgs e)
        {
            AdminCP.edtStaFromAdm = true;
            Accounts acc = new Accounts();
            acc.Title = "لوحه الحالة";
            acc.Show();
        }
        /// <summary>
        /// open bills page
        /// </summary>
        private void MIBills_Click(object sender, RoutedEventArgs e)
        {
           AdminCP.IsBills = true;
        }
        /// <summary>
        /// open all drugs page
        /// </summary>
        private void MIAllMeds_Click(object sender, RoutedEventArgs e)
        {
            AdminCP.IsAll = true;
        }
        /// <summary>
        /// open end expiration page
        /// </summary>
        private void MIExy_Click(object sender, RoutedEventArgs e)
        {
            AdminCP.IsEX = true;
        }
        /// <summary>
        /// open end SoldLog page
        /// </summary>
        private void MISO_Click(object sender, RoutedEventArgs e)
        {
            AdminCP.IsSO = true;
        }
        #endregion

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

        #region nubmers only
        private void MPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void MExist_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void MWSell_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        #endregion

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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Accounts acc = new Accounts();
            acc.Title = "لوحه التحكم";
            acc.Show();
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
                        if (Convert.ToDateTime(MEX.Text) <= DateTime.Now.Date)
                        {
                            MEX.Background = Brushes.Red;
                            MEX.Foreground = Brushes.OrangeRed;
                            Console.WriteLine("Searched for - " + MName.Text + " - I believe that you should get rid of it");
                        }
                        if (Convert.ToDecimal(MExist.Text) <= 0)
                        {
                            MExist.Background = Brushes.Red;
                            MExist.Foreground = Brushes.White;
                            Console.WriteLine("Searched for - " + MName.Text + " - I believe that you should get new ones");
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
                        MExist.Text = r.ReadUInt32("Total").ToString();
                        MPrice.Text = r.ReadString("SPrice");
                        MEX.Text = r.ReadString("ExpirationDate");
                        MNotes.Text = r.ReadString("Notes");
                        SearchBox.Foreground = Brushes.Green;
                        if (Convert.ToDateTime(MEX.Text) <= DateTime.Now.Date)
                        {
                            MEX.Background = Brushes.Red;
                            MEX.Foreground = Brushes.OrangeRed;
                            Console.WriteLine("Searched for - " + MName.Text + " - I believe that you should get rid of it");
                        }
                        if (Convert.ToDecimal(MExist.Text) <= 0)
                        {
                            MExist.Background = Brushes.Red;
                            MExist.Foreground = Brushes.White;
                            Console.WriteLine("Searched for - " + MName.Text + " - I believe that you should get new ones");
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
            if (SearchBox.IsDropDownOpen == false && SearchBox.Text.Length > 0)
            {
                SearchBox.Foreground = Brushes.Blue;
                SearchBox.Items.Clear();
                SearchBox.IsDropDownOpen = true;
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            SearchBox.IsDropDownOpen = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("medics").WhereLike("Name", SearchBox.Text);
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read())
                {
                    if (!SearchBox.Items.Contains(r.ReadString("Name")))
                    {
                        SearchBox.Items.Add(r.ReadString("Name"));
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        private void SellM_Click(object sender, RoutedEventArgs e)
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
            if (Kernel.Core.aa == "1")
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("logs").Set("LogoutDate", DateTime.Now.ToString()).Set("Online", 0).Where("Online", 1).Execute();
            }
            CP.IsClosing = true;
        }

        #region Add Drugs Panel
        //back to main menu
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            AddNewDrugBoard.Visibility = Visibility.Hidden;
            menu1.IsTabStop = true;
            MeEmp.IsTabStop = true;
            MECul.IsTabStop = true;
            MeSto.IsTabStop = true;
            MeHe.IsTabStop = true;
            SearchBox.IsTabStop = true;
            SearchButton.IsTabStop = true;
            SearchButton.IsDefault = true;
            ByBarCode.IsTabStop = true;
            MName.IsTabStop = true;
            MSS.IsTabStop = true;
            MEX.Visibility = Visibility.Visible;
            MPrice.IsTabStop = true;
            MExist.IsTabStop = true;
            MWSell.IsTabStop = true;
            MType.IsTabStop = true;
            NewBill.IsTabStop = true;
            BillNo.IsTabStop = true;
            Client.IsTabStop = true;
            MNotes.IsTabStop = true;
            SellM.IsTabStop = true;
            UpdateM.IsTabStop = true;
            LogOut.IsTabStop = true;
            LogOut.IsCancel = true;
            AddDrug.IsDefault = false;
            BackToMain.IsCancel = false;
            AddDrug.IsTabStop = false;
            BackToMain.IsTabStop = false;
            ADName.IsTabStop = false;
            ADBarCode.IsTabStop = false;
            ADSS.IsTabStop = false;
            ADEXP.IsTabStop = false;
            ADPrice.IsTabStop = false;
            ADTotal.IsTabStop = false;
            ADType.IsTabStop = false;
            ADNote.IsTabStop = false;
            SearchBox.Focus();
        }
        //Background colors back
        void BGGB()
        {
            LinearGradientBrush ng = new LinearGradientBrush();
            ng.StartPoint = new Point(0.5, 0);
            ng.EndPoint = new Point(0.5, 1);
            ng.MappingMode = BrushMappingMode.RelativeToBoundingBox;
            GradientStop gs1 = new GradientStop();
            //#FF3630B4
            gs1.Color = Color.FromRgb(54, 48, 180);
            gs1.Offset = 0.833;
            ng.GradientStops.Add(gs1);
            GradientStop gs2 = new GradientStop();
            //#FF151083
            gs2.Color = Color.FromRgb(21, 16, 131);
            gs2.Offset = 1;
            ng.GradientStops.Add(gs2);
            AddNewDrugBoard.Background = ng;
        }
        // add drug panel
        private void MIAddDrug_Click(object sender, RoutedEventArgs e)
        {
            AddNewDrugBoard.Visibility = Visibility.Visible;
            ADName.Focus();
            menu1.IsTabStop = false;
            MeEmp.IsTabStop = false;
            MECul.IsTabStop = false;
            MeSto.IsTabStop = false;
            MeHe.IsTabStop = false;
            SearchBox.IsTabStop = false;
            SearchButton.IsTabStop = false;
            SearchButton.IsDefault = false;
            ByBarCode.IsTabStop = false;
            MName.IsTabStop = false;
            MSS.IsTabStop = false;
            MEX.Visibility = Visibility.Hidden;
            MPrice.IsTabStop = false;
            MExist.IsTabStop = false;
            MWSell.IsTabStop = false;
            MType.IsTabStop = false;
            NewBill.IsTabStop = false;
            BillNo.IsTabStop = false;
            Client.IsTabStop = false;
            MNotes.IsTabStop = false;
            SellM.IsTabStop = false;
            UpdateM.IsTabStop = false;
            LogOut.IsTabStop = false;
            LogOut.IsCancel = false;
            AddDrug.IsDefault = true;
            BackToMain.IsCancel = true;
            AddDrug.IsTabStop = true;
            BackToMain.IsTabStop = true;
            ADName.IsTabStop = true;
            ADBarCode.IsTabStop = true;
            ADSS.IsTabStop = true;
            ADEXP.IsTabStop = true;
            ADPrice.IsTabStop = true;
            ADTotal.IsTabStop = true;
            ADType.IsTabStop = true;
            ADType.SelectedIndex = 0;
            ADNote.IsTabStop = true;
            BGGB();
        }

        private void ADName_KeyDown(object sender, KeyEventArgs e)
        {
            if (ADName.Text == "أسم الدواء*")
            {
                ADName.Text = "";
            }
            ADName.IsDropDownOpen = true;
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("medics").WhereLike("Name", SearchBox.Text);
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read())
                {
                    if (!ADName.Items.Contains(r.ReadString("Name")))
                    {
                        ADName.Items.Add(r.ReadString("Name"));
                    }
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
            BGGB();
        }
        
        private void ADName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADName.Text == "")
            {
                ADName.Text = "أسم الدواء*";
            }
        }

        private void ADBarCode_GotFocus(object sender, RoutedEventArgs e)
        {
            ADBarCode.Foreground = Brushes.Blue;
            ADBarCode.Background = Brushes.White;
            if (ADBarCode.Text == "الباركود")
            {
                ADBarCode.Text = "";
            }
            BGGB();
        }

        private void ADBarCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADBarCode.Text == "")
            {
                ADBarCode.Text = "الباركود";
            }
        }

        private void ADSS_GotFocus(object sender, RoutedEventArgs e)
        {
            ADSS.Foreground = Brushes.Blue;
            ADSS.Background = Brushes.White;
            if (ADSS.Text == "المادة الفعاله")
            {
                ADSS.Text = "";
            }
            BGGB();
        }

        private void ADSS_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADSS.Text == "")
            {
                ADSS.Text = "المادة الفعاله";
            }
        }

        private void ADEXP_GotFocus(object sender, RoutedEventArgs e)
        {
            ADEXP.Foreground = Brushes.Blue;
            ADEXP.Background = Brushes.White;
            if (ADEXP.Text == "تاريخ انتهاء الصلاحيه*")
            {
                ADEXP.Text = "";
            }
            BGGB();
        }

        private void ADEXP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADEXP.Text == "")
            {
                ADEXP.Text = "تاريخ انتهاء الصلاحيه*";
            }
        }

        private void ADPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            ADPrice.Foreground = Brushes.Blue;
            ADPrice.Background = Brushes.White;
            if (ADPrice.Text == "سعر الدواء*")
            {
                ADPrice.Text = "";
            }
            BGGB();
        }

        private void ADPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADPrice.Text == "")
            {
                ADPrice.Text = "سعر الدواء*";
            }
        }

        private void ADTotal_GotFocus(object sender, RoutedEventArgs e)
        {
            ADTotal.Foreground = Brushes.Blue;
            ADTotal.Background = Brushes.White;
            if (ADTotal.Text == "الكميه*")
            {
                ADTotal.Text = "";
            }
            BGGB();
        }

        private void ADTotal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADTotal.Text == "")
            {
                ADTotal.Text = "الكميه*";
            }
        }

        private void ADType_GotFocus(object sender, RoutedEventArgs e)
        {
            ADType.Foreground = Brushes.Blue;
            ADType.Background = Brushes.White;
            if (ADType.Text == "النوع*")
            {
                ADType.Text = "";
            }
            BGGB();
        }

        private void ADType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADType.Text == "")
            {
                ADType.Text = "النوع*";
            }
        }

        private void ADNote_GotFocus(object sender, RoutedEventArgs e)
        {
            ADNote.Foreground = Brushes.Blue;
            ADNote.Background = Brushes.White;
            if (ADNote.Text == "ملاحظات")
            {
                ADNote.Text = "";
            }
            BGGB();
        }

        private void ADNote_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ADNote.Text == "")
            {
                ADNote.Text = "ملاحظات";
            }
        }

        private void ADPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void ADTotal_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                ADEXP.Background = Brushes.Red;
                return;
            }
            else if (ADEXP.SelectedDate <= DateTime.Now.Date)
            {
                ADEXP.Background = Brushes.Red;
                return;
            }
            if (ADPrice.Text == "سعر الدواء*")
            {
                ADPrice.Foreground = Brushes.Red;
                return;
            }
            else if (ADPrice.Text == "")
            {
                ADPrice.Background = Brushes.Red;
                return;
            }
            else if (Convert.ToDecimal(ADPrice.Text) <= 0)
            {
                ADPrice.Foreground = Brushes.Red;
                return;
            }
            if (ADTotal.Text == "الكميه*")
            {
                ADTotal.Foreground = Brushes.Red;
                return;
            }
            else if (ADTotal.Text == "")
            {
                ADTotal.Background = Brushes.Red;
                return;
            }
            else if (Convert.ToDecimal(ADTotal.Text) <= 0)
            {
                ADTotal.Foreground = Brushes.Red;
                return;
            }
            if (ADBarCode.Text == "الباركود")
            {
                ADBarCode.Text = "";
            }
            if (ADSS.Text == "المادة الفعاله")
            {
                ADSS.Text = "";
            }
            if (ADNote.Text == "ملاحظات")
            {
                ADNote.Text = "";
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
                LinearGradientBrush ng = new LinearGradientBrush();
                ng.StartPoint = new Point(0.5, 0);
                ng.EndPoint = new Point(0.5, 1);
                ng.MappingMode = BrushMappingMode.RelativeToBoundingBox;
                GradientStop gs1 = new GradientStop();
                gs1.Color = Color.FromRgb(5, 124, 48);
                gs1.Offset = 0.833;
                ng.GradientStops.Add(gs1);
                GradientStop gs2 = new GradientStop();
                gs2.Color = Color.FromRgb(0, 64, 24);
                gs2.Offset = 1;
                ng.GradientStops.Add(gs2);
                AddNewDrugBoard.Background = ng;
                Console.WriteLine(AccountsTable.UserName + " add " + ADTotal.Text + " " + ADName.Text + " which each cost " + ADPrice.Text);
                var later = DateTime.Now.Second;
            }
            catch (Exception ex)
            {
                AddNewDrugBoard.Background = Brushes.Red;
                Kernel.Core.SaveException(ex);
            }
        }
        #endregion

    }
}