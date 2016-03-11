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
using System.Windows.Threading;

namespace ProPharmacyManagerW.Pages
{
    /// <summary>
    /// Interaction logic for Expiration.xaml
    /// </summary>
    public partial class Expiration : Page
    {
        public Expiration()
        {
            InitializeComponent();
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
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                var mT = new System.Data.DataTable();
                mT.Columns.Add("الدواء", typeof (string));
                mT.Columns.Add("تاريخ انتهاء الصلاحية", typeof(string));
                mT.Columns.Add("السعر", typeof(decimal));
                mT.Columns.Add("الكمية", typeof(decimal));
                cmd.Select("medics");
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read())
                {
                    try
                    {
                        if (Convert.ToDateTime(r.ReadString("ExpirationDate")) <= DateTime.Now.Date)
                        {
                            mT.Rows.Add(r.ReadString("Name"), r.ReadString("ExpirationDate"), r.ReadDecimal("SPrice"), r.ReadDecimal("Total"));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("غالبا هناك خطا فى التواريخ");
                        Kernel.Core.SaveException(ex);
                        break;
                    }
                }
                dataGrid.ItemsSource = mT.DefaultView;
                r.Close();
                Pb.Visibility = Visibility.Hidden;
            });
        }

        private void BackMainB_Click(object sender, RoutedEventArgs e)
        {
            CP.BackToMain = true;
        }
    }
}
