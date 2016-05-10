// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProPharmacyManagerW.Pages
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
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                Pb.Height = 10;
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Pb.Visibility = Visibility.Visible;
                if (dataGrid.Columns.Count > 3)
                {
                    return;
                }
                var mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medics", DataHolder.MySqlConnection);
                var mT = new System.Data.DataTable();
                mA.Fill(mT);
                if (mT.Rows.Count == 0)
                {
                    mT.Rows.Add(new object[mT.Columns.Count]);
                }
                if (mT.Rows.Count < 2)
                {
                    MessageBox.Show("اضف بعض الادويه اولا");
                    BackMainB_Click(sender, e);
                    return;
                }
                mT.Columns["Name"].ColumnName = "اسم الدواء";
                mT.Columns["Barcode"].ColumnName = "الباركود";
                mT.Columns["ScientificName"].ColumnName = "المادة الفعالة";
                mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                mT.Columns["Type"].ColumnName = "النوع";
                mT.Columns["Total"].ColumnName = "الكمية";
                mT.Columns["SPrice"].ColumnName = "سعر البيع";
                mT.Columns["Notes"].ColumnName = "ملاحظات";
                dataGrid.ItemsSource = mT.DefaultView;
                dataGrid.Columns[0].DisplayIndex = 7;
                dataGrid.Columns[1].DisplayIndex = 4;
                dataGrid.Columns[5].Visibility = Visibility.Collapsed;
                dataGrid.Columns[7].Visibility = Visibility.Collapsed;
            });
        }
        
        private void SearchB_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                if (cByName.IsChecked == true)
                {
                    Pb.Visibility = Visibility.Visible;
                    mT.Clear();
                    mA.Dispose();
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
                        mT.Columns["Name"].ColumnName = "اسم الدواء";
                    }
                    if (mT.Columns.Contains("Barcode"))
                    {
                        mT.Columns["Barcode"].ColumnName = "الباركود";
                    }
                    if (mT.Columns.Contains("ScientificName"))
                    {
                        mT.Columns["ScientificName"].ColumnName = "المادة الفعالة";
                    }
                    if (mT.Columns.Contains("ExpirationDate"))
                    {
                        mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                    }
                    if (mT.Columns.Contains("Type"))
                    {
                        mT.Columns["Type"].ColumnName = "النوع";
                    }
                    if (mT.Columns.Contains("Total"))
                    {
                        mT.Columns["Total"].ColumnName = "الكمية";
                    }
                    if (mT.Columns.Contains("SPrice"))
                    {
                        mT.Columns["SPrice"].ColumnName = "السعر";
                    }
                    if (mT.Columns.Contains("Notes"))
                    {
                        mT.Columns["Notes"].ColumnName = "ملاحظات";
                    }
                    dataGrid.ItemsSource = mT.DefaultView;
                    Pb.Visibility = Visibility.Hidden;
                }
                else if (cByBar.IsChecked == true)
                {
                    Pb.Visibility = Visibility.Visible;
                    mT.Clear();
                    mA.Dispose();
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
                        mT.Columns["Name"].ColumnName = "اسم الدواء";
                    }
                    if (mT.Columns.Contains("Barcode"))
                    {
                        mT.Columns["Barcode"].ColumnName = "الباركود";
                    }
                    if (mT.Columns.Contains("ScientificName"))
                    {
                        mT.Columns["ScientificName"].ColumnName = "المادة الفعالة";
                    }
                    if (mT.Columns.Contains("ExpirationDate"))
                    {
                        mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                    }
                    if (mT.Columns.Contains("Type"))
                    {
                        mT.Columns["Type"].ColumnName = "النوع";
                    }
                    if (mT.Columns.Contains("Total"))
                    {
                        mT.Columns["Total"].ColumnName = "الكمية";
                    }
                    if (mT.Columns.Contains("SPrice"))
                    {
                        mT.Columns["SPrice"].ColumnName = "السعر";
                    }
                    if (mT.Columns.Contains("Notes"))
                    {
                        mT.Columns["Notes"].ColumnName = "ملاحظات";
                    }
                    dataGrid.ItemsSource = mT.DefaultView;
                    Pb.Visibility = Visibility.Hidden;
                }
                else if (cBySub.IsChecked == true)
                {
                    Pb.Visibility = Visibility.Visible;
                    mT.Clear();
                    mA.Dispose();
                    mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medics WHERE `ScientificName` = '" + SearchBox.Text + "'ORDER BY Name", Database.DataHolder.MySqlConnection);
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
                        mT.Columns["Name"].ColumnName = "اسم الدواء";
                    }
                    if (mT.Columns.Contains("Barcode"))
                    {
                        mT.Columns["Barcode"].ColumnName = "الباركود";
                    }
                    if (mT.Columns.Contains("ScientificName"))
                    {
                        mT.Columns["ScientificName"].ColumnName = "المادة الفعالة";
                    }
                    if (mT.Columns.Contains("ExpirationDate"))
                    {
                        mT.Columns["ExpirationDate"].ColumnName = "تاريخ انتهاء الصلاحية";
                    }
                    if (mT.Columns.Contains("Type"))
                    {
                        mT.Columns["Type"].ColumnName = "النوع";
                    }
                    if (mT.Columns.Contains("Total"))
                    {
                        mT.Columns["Total"].ColumnName = "الكمية";
                    }
                    if (mT.Columns.Contains("SPrice"))
                    {
                        mT.Columns["SPrice"].ColumnName = "السعر";
                    }
                    if (mT.Columns.Contains("Notes"))
                    {
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
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (ThreadStart)delegate ()
                {
                    if (cByName.IsChecked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                        cmd.Select("medics").WhereLike("Name", SearchBox.Text);
                        MySqlReader r = new MySqlReader(cmd);
                        while (r.Read() && SearchBox.Items.Count <= 10 && SearchBox.Text.Length > 0)
                        {
                            SearchBox.Items.Add(r.ReadString("Name"));
                        }
                    }
                    else if (cByBar.IsChecked == true)
                    {
                        SearchBox.Items.Clear();
                    }
                    else if (cBySub.IsChecked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                        cmd.Select("medics").WhereLike("ScientificName", SearchBox.Text);
                        MySqlReader r = new MySqlReader(cmd);
                        while (r.Read() && SearchBox.Items.Count <= 10 && SearchBox.Text.Length > 0)
                        {
                            if (!SearchBox.Items.Contains(r.ReadString("ScientificName")))
                            {
                                SearchBox.Items.Add(r.ReadString("ScientificName"));
                            }
                        }
                    }

                });
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
                SearchBox.Items.Clear();
                SearchBox.IsDropDownOpen = true;
            }
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.IsReadOnly = true;
        }
    }
}
