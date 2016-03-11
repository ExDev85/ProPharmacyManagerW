// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Database;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProPharmacyManager.Pages
{
    /// <summary>
    /// Interaction logic for AllMeds.xaml
    /// </summary>
    public partial class AllMeds : Page
    {
        public AllMeds()
        {
            InitializeComponent();
        }

        MySql.Data.MySqlClient.MySqlDataAdapter mA;
        System.Data.DataTable mT;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Pb.Visibility = Visibility.Visible;
                mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medics ORDER BY Name", DataHolder.MySqlConnection);
                mT = new System.Data.DataTable();
                mA.Fill(mT);
                if (mA == null)
                    return;
                if (mT.Rows.Count == 0)
                {
                    mT.Rows.Add(new object[mT.Columns.Count]);
                }
                if (mT.Columns.Contains("Name"))
                {
                    mT.Columns["Name"].ReadOnly = true;
                    mT.Columns["Name"].ColumnName = "اسم الدواء";
                }
                if (mT.Columns.Contains("Barcode"))
                {
                    mT.Columns["Barcode"].ReadOnly = true;
                    mT.Columns["Barcode"].ColumnName = "الباركود";
                }
                if (mT.Columns.Contains("ActivePrinciple"))
                {
                    mT.Columns["ActivePrinciple"].ReadOnly = true;
                    mT.Columns["ActivePrinciple"].ColumnName = "المادة الفعالة";
                }
                if (mT.Columns.Contains("ExpirationDate"))
                {
                    mT.Columns["ExpirationDate"].ReadOnly = true;
                    mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                }
                if (mT.Columns.Contains("Type"))
                {
                    mT.Columns["Type"].ReadOnly = true;
                    mT.Columns["Type"].ColumnName = "النوع";
                }
                if (mT.Columns.Contains("Total"))
                {
                    mT.Columns["Total"].ReadOnly = true;
                    mT.Columns["Total"].ColumnName = "الكمية";
                }
                if (mT.Columns.Contains("Price"))
                {
                    mT.Columns["Price"].ReadOnly = true;
                    mT.Columns["Price"].ColumnName = "السعر";
                }
                if (mT.Columns.Contains("Notes"))
                {
                    mT.Columns["Notes"].ReadOnly = true;
                    mT.Columns["Notes"].ColumnName = "ملاحظات";
                }
                dataGrid.ItemsSource = mT.DefaultView;
                Pb.Visibility = Visibility.Hidden;
            });
            cByName.IsChecked = true;
        }

        #region CheckState Group
        private void cByName_Checked(object sender, RoutedEventArgs e)
        {
            if (cByName.IsChecked == true)
            {
                cByBar.IsChecked = false;
                cBySub.IsChecked = false;
            }
        }

        private void cByBar_Checked(object sender, RoutedEventArgs e)
        {
            if (cByBar.IsChecked == true)
            {
                cByName.IsChecked = false;
                cBySub.IsChecked = false;
            }
        }

        private void cBySub_Checked(object sender, RoutedEventArgs e)
        {
            if (cBySub.IsChecked == true)
            {
                cByName.IsChecked = false;
                cByBar.IsChecked = false;
            }
        }
        #endregion

        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                if (cByName.IsChecked == true)
                {
                    Pb.Visibility = Visibility.Visible;
                    mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medics WHERE `Name` = '" + SearchBox.Text + "'ORDER BY Name", Database.DataHolder.MySqlConnection);
                    mT = new System.Data.DataTable();
                    mA.Fill(mT);
                    if (mA == null)
                        return;
                    if (mT.Rows.Count == 0)
                    {
                        mT.Rows.Add(new object[mT.Columns.Count]);
                    }
                    if (mT.Columns.Contains("Name"))
                    {
                        mT.Columns["Name"].ReadOnly = true;
                        mT.Columns["Name"].ColumnName = "اسم الدواء";
                    }
                    if (mT.Columns.Contains("Barcode"))
                    {
                        mT.Columns["Barcode"].ReadOnly = true;
                        mT.Columns["Barcode"].ColumnName = "الباركود";
                    }
                    if (mT.Columns.Contains("ActivePrinciple"))
                    {
                        mT.Columns["ActivePrinciple"].ReadOnly = true;
                        mT.Columns["ActivePrinciple"].ColumnName = "المادة الفعالة";
                    }
                    if (mT.Columns.Contains("ExpirationDate"))
                    {
                        mT.Columns["ExpirationDate"].ReadOnly = true;
                        mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                    }
                    if (mT.Columns.Contains("Type"))
                    {
                        mT.Columns["Type"].ReadOnly = true;
                        mT.Columns["Type"].ColumnName = "النوع";
                    }
                    if (mT.Columns.Contains("Total"))
                    {
                        mT.Columns["Total"].ReadOnly = true;
                        mT.Columns["Total"].ColumnName = "الكمية";
                    }
                    if (mT.Columns.Contains("Price"))
                    {
                        mT.Columns["Price"].ReadOnly = true;
                        mT.Columns["Price"].ColumnName = "السعر";
                    }
                    if (mT.Columns.Contains("Notes"))
                    {
                        mT.Columns["Notes"].ReadOnly = true;
                        mT.Columns["Notes"].ColumnName = "ملاحظات";
                    }
                    dataGrid.ItemsSource = mT.DefaultView;
                    Pb.Visibility = Visibility.Hidden;
                }
                else if (cByBar.IsChecked == true)
                {
                    Pb.Visibility = Visibility.Visible;
                    mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medics WHERE `Barcode` = '" + SearchBox.Text + "'ORDER BY Name", Database.DataHolder.MySqlConnection);
                    mT = new System.Data.DataTable();
                    mA.Fill(mT);
                    if (mA == null)
                        return;
                    if (mT.Rows.Count == 0)
                    {
                        mT.Rows.Add(new object[mT.Columns.Count]);
                    }
                    if (mT.Columns.Contains("Name"))
                    {
                        mT.Columns["Name"].ReadOnly = true;
                        mT.Columns["Name"].ColumnName = "اسم الدواء";
                    }
                    if (mT.Columns.Contains("Barcode"))
                    {
                        mT.Columns["Barcode"].ReadOnly = true;
                        mT.Columns["Barcode"].ColumnName = "الباركود";
                    }
                    if (mT.Columns.Contains("ActivePrinciple"))
                    {
                        mT.Columns["ActivePrinciple"].ReadOnly = true;
                        mT.Columns["ActivePrinciple"].ColumnName = "المادة الفعالة";
                    }
                    if (mT.Columns.Contains("ExpirationDate"))
                    {
                        mT.Columns["ExpirationDate"].ReadOnly = true;
                        mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                    }
                    if (mT.Columns.Contains("Type"))
                    {
                        mT.Columns["Type"].ReadOnly = true;
                        mT.Columns["Type"].ColumnName = "النوع";
                    }
                    if (mT.Columns.Contains("Total"))
                    {
                        mT.Columns["Total"].ReadOnly = true;
                        mT.Columns["Total"].ColumnName = "الكمية";
                    }
                    if (mT.Columns.Contains("Price"))
                    {
                        mT.Columns["Price"].ReadOnly = true;
                        mT.Columns["Price"].ColumnName = "السعر";
                    }
                    if (mT.Columns.Contains("Notes"))
                    {
                        mT.Columns["Notes"].ReadOnly = true;
                        mT.Columns["Notes"].ColumnName = "ملاحظات";
                    }
                    dataGrid.ItemsSource = mT.DefaultView;
                    Pb.Visibility = Visibility.Hidden;
                }
                else if (cBySub.IsChecked == true)
                {
                    Pb.Visibility = Visibility.Visible;
                    mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medics WHERE `ActivePrinciple` = '" + SearchBox.Text + "'ORDER BY Name", Database.DataHolder.MySqlConnection);
                    mT = new System.Data.DataTable();
                    mA.Fill(mT);
                    if (mA == null)
                        return;
                    if (mT.Rows.Count == 0)
                    {
                        mT.Rows.Add(new object[mT.Columns.Count]);
                    }
                    if (mT.Columns.Contains("Name"))
                    {
                        mT.Columns["Name"].ReadOnly = true;
                        mT.Columns["Name"].ColumnName = "اسم الدواء";
                    }
                    if (mT.Columns.Contains("Barcode"))
                    {
                        mT.Columns["Barcode"].ReadOnly = true;
                        mT.Columns["Barcode"].ColumnName = "الباركود";
                    }
                    if (mT.Columns.Contains("ActivePrinciple"))
                    {
                        mT.Columns["ActivePrinciple"].ReadOnly = true;
                        mT.Columns["ActivePrinciple"].ColumnName = "المادة الفعالة";
                    }
                    if (mT.Columns.Contains("ExpirationDate"))
                    {
                        mT.Columns["ExpirationDate"].ReadOnly = true;
                        mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                    }
                    if (mT.Columns.Contains("Type"))
                    {
                        mT.Columns["Type"].ReadOnly = true;
                        mT.Columns["Type"].ColumnName = "النوع";
                    }
                    if (mT.Columns.Contains("Total"))
                    {
                        mT.Columns["Total"].ReadOnly = true;
                        mT.Columns["Total"].ColumnName = "الكمية";
                    }
                    if (mT.Columns.Contains("Price"))
                    {
                        mT.Columns["Price"].ReadOnly = true;
                        mT.Columns["Price"].ColumnName = "السعر";
                    }
                    if (mT.Columns.Contains("Notes"))
                    {
                        mT.Columns["Notes"].ReadOnly = true;
                        mT.Columns["Notes"].ColumnName = "ملاحظات";
                    }
                    dataGrid.ItemsSource = mT.DefaultView;
                    Pb.Visibility = Visibility.Hidden;
                }
            });
        }

        private void UpdateB_Click(object sender, RoutedEventArgs e)
        {
            Page_Loaded(sender, e);
            Pb.Visibility = Visibility.Visible;
        }

        private void BackMainB_Click(object sender, RoutedEventArgs e)
        {
            CP.BackToMain = true;
        }

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            SearchBox.IsDropDownOpen = true;
            try
            {
                if (cByName.IsChecked == true)
                {
                    SearchBox.Items.Clear();
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("medics").WhereLike("Name", SearchBox.Text);
                    MySqlReader r = new MySqlReader(cmd);
                    while (r.Read())
                    {
                        SearchBox.Items.Add(r.ReadString("Name"));
                        SearchBox.Items.Refresh();
                    }
                }
                else if (cByBar.IsChecked == true)
                {
                    SearchBox.Items.Clear();
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("medics").WhereLike("Barcode", SearchBox.Text);
                    MySqlReader r = new MySqlReader(cmd);
                    while (r.Read())
                    {
                        SearchBox.Items.Add(r.ReadString("Barcode"));
                        SearchBox.Items.Refresh();
                    }
                }
                else if (cBySub.IsChecked == true)
                {
                    SearchBox.Items.Clear();
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("medics").WhereLike("ActivePrinciple", SearchBox.Text);
                    MySqlReader r = new MySqlReader(cmd);
                    while (r.Read())
                    {
                        SearchBox.Items.Add(r.ReadString("ActivePrinciple"));
                        SearchBox.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.IsDropDownOpen == false && SearchBox.Text.Length > 0)
            {
                SearchBox.Foreground = Brushes.Blue;
                SearchBox.IsDropDownOpen = true;
            }
        }
    }
}
