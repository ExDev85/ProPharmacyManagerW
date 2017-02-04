using ProPharmacyManagerW.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for AddSup.xaml
    /// </summary>
    public partial class AddSup : Page
    {
        public AddSup()
        {
            InitializeComponent();
        }

        void ReloadList()
        {
            SupList.Items.Clear();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("suppliers");
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                SupList.Items.Add(r.ReadString("Name"));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }

        private void AddB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("suppliers").Insert("Name", SupName.Text).Insert("Salesman", SupMan.Text).Insert("Phones", SupPhones.Text).Insert("Notes", SupNotes.Text).Execute();
                Console.WriteLine("You add " + SupMan.Text + " from " + SupName.Text + " to suppliers");
                SupList.Items.Add(SupName.Text);
                MessageBox.Show("تم اضافه " + SupName.Text + " كموزع");
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
                MessageBox.Show("المستخدم موجود من قبل");
            }
        }
    }
}
