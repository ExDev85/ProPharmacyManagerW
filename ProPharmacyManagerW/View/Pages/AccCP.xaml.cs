// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for Accounts (Delete them or change passwords)
    /// </summary>
    public partial class AccCP : Page
    {
        public AccCP()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Load || Refresh the users list after make changes to them
        /// </summary>
        void ReloadList()
        {
            UNList.Items.Clear();
            if (AccountsTable.IsAdmin() == true)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("accounts");
                MySqlReader r = new MySqlReader(cmd);
                while (r.Read())
                {
                    UNList.Items.Add(r.ReadString("Username"));
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("accounts").Where("Username", AccountsTable.UserName);
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    UNList.Items.Add(r.ReadString("Username"));
                }
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadList();
            if (AccountsTable.IsAdmin() == false)
            {
                AdminPCB.IsChecked = false;
                AdminPCB.Visibility = Visibility.Hidden;
                DelB.Visibility = Visibility.Hidden;
            }
        }

        private void EdtB_Click(object sender, RoutedEventArgs e)
        {
            // Be able to change any user password without knowing the old one because of Admin privileges
            if (AccountsTable.IsAdmin() == true && AdminPCB.IsChecked == true)
            {
                if (NUP1.Password == NUP2.Password)
                {
                    try
                    {
                        MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                        cmd2.Update("accounts").Set("Password", Kernel.Core.GetSHAHashData(NUP1.Password)).Where("Username", UNList.SelectedItem.ToString()).Execute();
                        label1.Content = "تم تغيير كلمه المرور بنجاح.";
                        label1.Foreground = Brushes.Green;
                        label1.Visibility = Visibility.Visible;
                        Console.WriteLine("You just changed " + UNList.SelectedItem + " password");
                    }
                    catch (Exception ex1)
                    {
                        label1.Content = "ليس هناك حساب بهذا الاسم.";
                        label1.Foreground = Brushes.Red;
                        label1.Visibility = Visibility.Visible;
                        Kernel.Core.SaveException(ex1);
                    }
                }
                else
                {
                    label1.Content = "كلمة المرور الجديدة و اعادتها غير متطابقين";
                    label1.Foreground = Brushes.Red;
                    label1.Visibility = Visibility.Visible;
                }
            }
            //Change the user password after typing the right old one without Admin privileges
            else
            {
                try
                {
                    MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd1.Select("accounts").Where("Username", UNList.SelectedItem.ToString()).And("Password", OUP.Text).Execute();
                    MySqlReader r = new MySqlReader(cmd1);
                    if (r.Read())
                    {
                        if (NUP1.Password == NUP2.Password)
                        {
                            try
                            {
                                MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                                cmd2.Update("accounts").Set("Password", Kernel.Core.GetSHAHashData(NUP1.Password)).Where("Username", UNList.SelectedItem.ToString()).Execute();
                                label1.Content = "تم تغيير كلمه المرور بنجاح.";
                                label1.Foreground = Brushes.Green;
                                label1.Visibility = Visibility.Visible;
                                Console.WriteLine(UNList.SelectedItem.ToString() + " password has changed");
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
                            label1.Content = "كلمة المرور الجديدة و اعادتها غير متطابقين";
                            label1.Foreground = Brushes.Red;
                            label1.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        label1.Content = "خطأ فى كلمة المرور القديمه";
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

        private void DelB_Click(object sender, RoutedEventArgs e)
        {
            //Delete any user Admin privileges requires
            if (AdminPCB.IsChecked == true)
            {
                try
                {
                    new MySqlCommand(MySqlCommandType.DELETE).Delete("accounts", "Username", UNList.SelectedItem.ToString()).Execute();
                    label1.Content = "تم حذف الحساب بنجاح.";
                    label1.Foreground = Brushes.Red;
                    label1.Visibility = Visibility.Visible;
                    Console.WriteLine("Good job you have just deleted " + UNList.SelectedItem.ToString());
                    ReloadList();
                }
                catch (Exception ex)
                {
                    label1.Content = "ليس هناك حساب بهذا الاسم.";
                    label1.Foreground = Brushes.Red;
                    label1.Visibility = Visibility.Visible;
                    Kernel.Core.SaveException(ex);
                }
            }
        }
    }
}
