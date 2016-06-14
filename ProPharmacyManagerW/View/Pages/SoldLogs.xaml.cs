// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.View;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for SoldLog.xaml
    /// </summary>
    public partial class SoldLogs : Page
    {
        public SoldLogs()
        {
            InitializeComponent();
        }

        private void BackMainB_Click(object sender, RoutedEventArgs e)
        {
            CP.BackToMain = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                Pb.Height = 10;
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Pb.Visibility = Visibility.Visible;
                var mA = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM medlog", DataHolder.MySqlConnection);
                var mT = new System.Data.DataTable();
                mA.Fill(mT);
                if (mA == null)
                    return;
                if (mT.Rows.Count == 0)
                {
                    mT.Rows.Add(new object[mT.Columns.Count]);
                }
                if (mT.Columns.Contains("MedName"))
                {
                    mT.Columns["MedName"].ReadOnly = true;
                    mT.Columns["MedName"].ColumnName = "اسم الدواء";
                }
                if (mT.Columns.Contains("SellDate"))
                {
                    mT.Columns["SellDate"].ReadOnly = true;
                    mT.Columns["SellDate"].ColumnName = "وقت البيع";
                }
                if (mT.Columns.Contains("TotalAmount"))
                {
                    mT.Columns["TotalAmount"].ReadOnly = true;
                    mT.Columns["TotalAmount"].ColumnName = "الكمية المباعة";
                }
                if (mT.Columns.Contains("TotalPrice"))
                {
                    mT.Columns["TotalPrice"].ReadOnly = true;
                    mT.Columns["TotalPrice"].ColumnName = "اجمالى السعر";
                }
                if (mT.Columns.Contains("Cashier"))
                {
                    mT.Columns["Cashier"].ReadOnly = true;
                    mT.Columns["Cashier"].ColumnName = "البائع";
                }
                dataGrid.ItemsSource = mT.DefaultView;
                Pb.Visibility = Visibility.Hidden;
            });
        }
    }
}
