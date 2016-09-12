// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.View.Pages;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View
{
    /// <summary>
    /// Interaction logic for CP.xaml
    /// </summary>
    public partial class CP : Window
    {
        public CP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if user want to get back to main cp
        /// </summary>
        public static bool BackToMain;

        /// <summary>
        /// Check if user logging out or not
        /// </summary>
        public static bool IsLoggingOut;

        private DispatcherTimer checkClosing = new DispatcherTimer();

        #region Window Status
        //The title bar to maxmize the form when user double click it or drag it by hold
        private void border1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    if (this.WindowState == WindowState.Normal)
                    {
                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                    }
                }
                else
                {
                    this.DragMove();
                }
            }
        }
        //X Button that close the current form
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsLoggingOut = true;
        }
        // - Button that minimize the current form
        private void imageButton1_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void PCP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch
            {
                Console.WriteLine("CP.xaml.cs LN79 error");
            }
        }
        #endregion

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
            FFhost.IsEnabled = false;
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
            FFhost.IsEnabled = false;
            StaCP sc = new StaCP();
            TwoPanelFame.Navigate(sc);
            UserBoard.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// open logs page for login processes
        /// </summary>
        private void MILOGLOG_Click(object sender, RoutedEventArgs e)
        {
            FFhost.IsEnabled = false;
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
            Settings set = new Settings();
            FFhost.Navigate(set);
        }
        /// <summary>
        /// open bills page
        /// </summary>
        public static bool IsBills;
        private void MIBills_Click(object sender, RoutedEventArgs e)
        {
            IsBills = true;
            Bills bl = new Bills();
            FFhost.Navigate(bl);
        }
        /// <summary>
        /// open all drugs page
        /// </summary>
        public static bool IsAll;
        private void MIAllMeds_Click(object sender, RoutedEventArgs e)
        {
            AllMeds am = new AllMeds();
            FFhost.Navigate(am);
        }
        /// <summary>
        /// open end expiration page
        /// </summary>
        public static bool IsEX;
        private void MIExy_Click(object sender, RoutedEventArgs e)
        {
            IsEX = true;
            Expiration ex = new Expiration();
            FFhost.Navigate(ex);
        }
        /// <summary>
        /// open Soldout page
        /// </summary>
        public static bool IsSO;
        private void MISO_Click(object sender, RoutedEventArgs e)
        {
            IsSO = true;
            Out so = new Out();
            FFhost.Navigate(so);
        }
        /// <summary>
        /// open end SoldLog page
        /// </summary>
        public static bool IsSL;
        private void MISM_Click(object sender, RoutedEventArgs e)
        {
            IsSL = true;
            SoldLogs sl = new SoldLogs();
            FFhost.Navigate(sl);
        }
        /// <summary>
        /// open end Backup and restore page
        /// </summary>
        public static bool IsBR;
        private void BAR_Click(object sender, RoutedEventArgs e)
        {
            IsBR = true;
            BAR br = new BAR();
            FFhost.Navigate(br);
        }
        #endregion

        private void PCP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                About abo = new About();
                abo.ShowDialog();
            }
            else if (e.Key == Key.F12)
            {
                if (Kernel.Core.IsCMode == false)
                {
                    ConGui consl = new ConGui();
                    consl.Show();
                }
            }
        }

        private void PCP_Loaded(object sender, RoutedEventArgs e)
        {
            if (AccountsTable.IsAdmin() == true)
            {
                this.Title = "لوحه المدراء";
            }
            else if (AccountsTable.IsAdmin() == false)
            {
                this.Title = "لوحه الموظفين";
                MeSe.Visibility = Visibility.Collapsed;
                MIAddEMP.Visibility = Visibility.Collapsed;
                MILOGLOG.Visibility = Visibility.Collapsed;
                MISM.Visibility = Visibility.Collapsed;
            }
            checkClosing.Interval = TimeSpan.FromMilliseconds(100);
            checkClosing.Tick += checkClosingState;
            checkClosing.Start();
        }

        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ".")
            {
                e.Handled = true;
            }
        }

        private void checkClosingState(object sender, EventArgs e)
        {
            if (IsLoggingOut == true)
            {
                //open login window after user logs out
                this.Close();
                MainWindow loginw = new MainWindow();
                if (IsLoaded == false)
                {
                    IsLoggingOut = false;
                    loginw.ShowDialog();
                    checkClosing.Stop();
                }
            }
            if (BackToMain == true)
            {
                MainCP ac = new MainCP();
                FFhost.Navigate(ac);
                if (AccountsTable.IsAdmin() == true)
                {
                    this.Title = "لوحه المدراء";
                }
                else if (AccountsTable.IsAdmin() == false)
                {
                    this.Title = "لوحه الموظفين";
                    MeSe.Visibility = Visibility.Collapsed;
                    MIAddEMP.Visibility = Visibility.Collapsed;
                    MILOGLOG.Visibility = Visibility.Collapsed;
                    MISM.Visibility = Visibility.Collapsed;
                }
                BackToMain = false;
            }
        }

        #region Add Drugs Panel
        //back to main menu
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            if (AddNewDrugBoard.Visibility == Visibility.Visible)
            {
                ADName.Text = "";
                ADBarCode.Text = "";
                ADSS.Text = "";
                ADEXP.Text = "";
                ADType.SelectedIndex = -1;
                ADTotal.Text = "";
                ADPrice.Text = "";
                ADNote.Text = "";
                AddNewDrugBoard.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserBoard.Visibility = Visibility.Collapsed;
            }
            FFhost.IsEnabled = true;
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
            FFhost.IsEnabled = false;
            AddNewDrugBoard.Visibility = Visibility.Visible;
            ADName.Focus();
            //#3399FF, #0066CC
            LGBB(51, 153, 255, 0, 102, 204);
        }

        private void ADName_KeyDown(object sender, KeyEventArgs e)
        {
            if (ADName.IsDropDownOpen == false && ADName.Text.Length > 0)
            {
                ADName.IsDropDownOpen = true;
            }
            try
            {
                if (ADName.Text.Length == 0)
                {
                    ADName.Items.Clear();
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

        private void RestoreBackground(object sender, RoutedEventArgs e)
        {
            //#3399FF, #0066CC
            LGBB(51, 153, 255, 0, 102, 204);
        }
        // add medic command
        private void AddDrug_Click(object sender, RoutedEventArgs e)
        {
            #region textboxs States
            if (ADName.Text == "")
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

        private void PCP_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Kernel.Core.aa == "1")
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("logs").Set("LogoutDate", DateTime.Now.ToString()).Set("Online", 0).Where("Online", 1).Execute();
            }
        }

    }
}
