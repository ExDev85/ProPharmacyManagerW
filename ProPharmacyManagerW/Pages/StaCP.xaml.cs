// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProPharmacyManager.Pages
{
    /// <summary>
    /// Interaction logic for Accounts (change phone numbers)
    /// </summary>
    public partial class StaCP : Page
    {
        public StaCP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the users list after make changes to them
        /// </summary>
        void LoadList()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("accounts");
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                UNList.Items.Add(r.ReadString("Username"));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadList();
            if (AccountsTable.IsAdmin() == false)
            {
                AdminPCB.IsChecked = false;
                AdminPCB.Visibility = Visibility.Hidden;
                RUState.IsReadOnly = true;
                RUState.IsEditable = false;
                RUState.Text = "موظف";
            }
        }

        private void EdtB_Click(object sender, RoutedEventArgs e)
        {
            byte state;
            switch (RUState.Text)
            {
                case "مدير":
                    state = 2;
                    break;
                case "موظف":
                    state = 1;
                    break;
                case "مجهول":
                    state = 0;
                    break;
                case "صلاحيات المستخدم":
                    state = 0;
                    break;
                default:
                    MessageBox.Show("اختار صلاحيات المستخدم");
                    return;
            }
            // Be able to change any user state without knowing the his password because of Admin privileges
            if (AccountsTable.IsAdmin() == true && AdminPCB.IsChecked == true)
            {
                try
                {
                    MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmd2.Update("accounts").Set("State", state).Set("Phone", PHN.Text).Where("Username", UNList.SelectedItem.ToString()).Execute();
                    label1.Content = "تم تغيير الحالة.";
                    label1.Foreground = Brushes.Green;
                    label1.Visibility = Visibility.Visible;
                    Console.WriteLine("You just changed " + UNList.SelectedItem + " States");
                }
                catch (Exception ex1)
                {
                    label1.Content = "ليس هناك حساب بهذا الاسم.";
                    label1.Foreground = Brushes.Red;
                    label1.Visibility = Visibility.Visible;
                    Kernel.Core.SaveException(ex1);
                }
            }
            //Change the user state after typing the password without Admin privileges
            else
            {
                try
                {
                    MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd1.Select("accounts").Where("Username", UNList.SelectedItem.ToString()).And("Password", Kernel.Core.GetSHAHashData(UP.Password)).Execute();
                    MySqlReader r = new MySqlReader(cmd1);
                    if (r.Read())
                    {
                        try
                        {
                            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                            cmd2.Update("accounts").Set("State", state).Set("Phone", PHN.Text).Where("Username", UNList.SelectedItem.ToString()).Execute();
                            label1.Content = "تم تغيير الحالة بنجاح.";
                            label1.Foreground = Brushes.Green;
                            label1.Visibility = Visibility.Visible;
                            Console.WriteLine(UNList.SelectedItem.ToString() + " changed his states");
                        }
                        catch (Exception ex2)
                        {
                            label1.Content = "ليس هناك حساب بهذا الاسم.";
                            label1.Foreground = Brushes.Red;
                            label1.Visibility = Visibility.Visible;
                            Kernel.Core.SaveException(ex2);
                        }
                    }
                    else
                    {
                        label1.Content = "خطأ فى كلمة المرور";
                        label1.Foreground = Brushes.Red;
                        label1.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex3)
                {
                    Kernel.Core.SaveException(ex3);
                }
            }
        }

        private void UNList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            label1.Visibility = Visibility.Hidden;
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd1.Select("accounts").Where("Username", UNList.SelectedItem.ToString()).Execute();
            MySqlReader r = new MySqlReader(cmd1);
            if (r.Read())
            {
                try
                {
                    byte state;
                    state = r.ReadByte("State");
                    switch (state)
                    {
                        case 2:
                            RUState.Text = "مدير";
                            break;
                        case 1:
                            RUState.Text = "موظف";
                            break;
                        case 0:
                            RUState.Text = "مجهول";
                            break;
                    }
                    RUState.SelectedItem = RUState.Text;
                    PHN.Text = r.ReadString("Phone");
                }
                catch (Exception ex)
                {
                    Kernel.Core.SaveException(ex);
                }
            }
        }

    }
}
