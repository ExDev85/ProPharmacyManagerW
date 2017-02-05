using ProPharmacyManagerW.Database;
using System;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// clear the list then gets suppliers names and ids
        /// </summary>
        void loadList()
        {
            SupList.Items.Clear();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("suppliers");
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                SupList.Items.Add(r.ReadString("Id") + "-" + r.ReadString("Name"));
            }
        }
        /// <summary>
        /// get supplier Id
        /// </summary>
        /// <returns>Id as string</returns>
        string getSupId()
        {
            string[] data = SupList.SelectedItem.ToString().Split('-');
            return data[0];
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadList();
        }

        private void SupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SupList.SelectedIndex > -1)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("suppliers").Where("Id", getSupId()).Execute();
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    SupName.Text = r.ReadString("Name");
                    SupMan.Text = r.ReadString("Salesman");
                    SupPhones.Text = r.ReadString("Phones");
                    SupNotes.Text = r.ReadString("Notes");
                }
            }
        }

        private void AddB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("suppliers").Insert("Name", SupName.Text).Insert("Salesman", SupMan.Text).Insert("Phones", SupPhones.Text).Insert("Notes", SupNotes.Text).Execute();
                Console.WriteLine("You add " + SupMan.Text + " from " + SupName.Text + " to suppliers");
                SupList.Items.Add(SupName.Text);
                MessageBox.Show("تم اضافه " + SupName.Text + " كمورد");
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
                MessageBox.Show("هناك مشكله فى المورد ربما يكون موجود من قبل");
            }
        }

        private void EdtB_Click(object sender, RoutedEventArgs e)
        {
            if (SupList.SelectedIndex > -1)
            {
                try
                {
                    MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmd2.Update("suppliers").Set("Name", SupName.Text).Set("Salesman", SupMan.Text).Set("Phones", SupPhones.Text).Set("Notes", SupNotes.Text).Where("id", getSupId()).Execute();
                    Console.WriteLine(AccountsTable.UserName + " just changed " + SupList.SelectedItem + " info");
                    MessageBox.Show("قمت بتحديث بيانات المورد");
                }
                catch (Exception ex1)
                {
                    Kernel.Core.SaveException(ex1);
                    MessageBox.Show("هناك مشكله ما فى عمليه التعديل");
                }
            }
            else
            {
                MessageBox.Show("اختار مورد ليتم تعديله");
            }
        }

        private void DelB_Click(object sender, RoutedEventArgs e)
        {
            if (SupList.SelectedIndex > -1)
            {
                try
                {
                    new MySqlCommand(MySqlCommandType.DELETE).Delete("suppliers", "Id", getSupId()).Execute();
                    Console.WriteLine("Good job you probably cost " + SupMan.Text + " from " + SupName.Text + "his job" + SupList.SelectedItem.ToString());
                    SupList.Items.Remove(SupList.SelectedItem);
                    MessageBox.Show("تم حذف المورد");
                }
                catch (Exception ex)
                {
                    Kernel.Core.SaveException(ex);
                    MessageBox.Show("مشكله فى عمليه الحذف ربما اسم خاطئ");
                }
            }
            else
            {
                MessageBox.Show("اختار مورد اولا ليتم حذفه");
            }
        }
    }
}
