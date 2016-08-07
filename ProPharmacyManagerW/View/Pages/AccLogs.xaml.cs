// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using System;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for AccLogs.xaml
    /// </summary>
    public partial class AccLogs : Page
    {
        public AccLogs()
        {
            InitializeComponent();
        }

        MySql.Data.MySqlClient.MySqlDataAdapter mAdapter;
        DataTable mTable;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                Pb.Height = 10;
            }
            Pb.Visibility = Visibility.Visible;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                mAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM logs", DataHolder.MySqlConnection);
                mTable = new DataTable();
                mAdapter.Fill(mTable);
                if (mTable.Rows.Count == 0)
                {
                    mTable.Rows.Add(new object[mTable.Columns.Count]);
                }
                if (mTable.Columns.Contains("Username"))
                {
                    mTable.Columns["Username"].ReadOnly = true;
                    mTable.Columns["Username"].ColumnName = "اسم الموظف";
                }
                if (mTable.Columns.Contains("LoginDate"))
                {
                    mTable.Columns["LoginDate"].ReadOnly = true;
                    mTable.Columns["LoginDate"].ColumnName = "وقت تسجيل الدخول";
                }
                if (mTable.Columns.Contains("LogoutDate"))
                {
                    mTable.Columns["LogoutDate"].ReadOnly = true;
                    mTable.Columns["LogoutDate"].ColumnName = "وقت تسجيل الخروج";
                }
                if (mTable.Columns.Contains("Online"))
                {
                    mTable.Columns["Online"].ReadOnly = true;
                    mTable.Columns["Online"].ColumnName = "متصل";
                }
                if (mAdapter == null)
                    return;
                dataGrid.ItemsSource = mTable.DefaultView;
            });
            Pb.Visibility = Visibility.Hidden;
        }
    }
}
