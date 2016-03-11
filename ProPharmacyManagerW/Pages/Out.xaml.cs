using ProPharmacyManagerW.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProPharmacyManagerW.Pages
{
    /// <summary>
    /// Interaction logic for Out.xaml
    /// </summary>
    public partial class Out : Page
    {
        public Out()
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
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                var mT = new System.Data.DataTable();
                mT.Columns.Add("الدواء", typeof(string));
                cmd.Select("medics").Where("Total", 0);
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read())
                {
                    mT.Rows.Add(r.ReadString("Name"));
                }
                dataGrid.ItemsSource = mT.DefaultView;
                r.Close();
                Pb.Visibility = Visibility.Hidden;
            });
        }
    }
}
