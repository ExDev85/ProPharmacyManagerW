// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// To add new employee
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }
        /// <summary>
        /// check if user insert the new user to the database without problems
        /// </summary>
        public static bool IsRegisCom;

        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void InsReg_Click(object sender, RoutedEventArgs e)
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
                case "صلاحيات المستخدم*":
                    state = 0;
                    break;
                default:
                    MessageBox.Show("اختار صلاحيات المستخدم");
                    return;
            }
            if (RUName.Text == "")
            {
                RUName.Background = Brushes.Red;
                return;
            }
            else
            {
                RUName.Background = Brushes.White;
            }
            // حشسسصخقي = password with arabic letters
            if ((RUPass1.Password == "" || RUPass1.Password == "حشسسصخقي") || (RUPass2.Password == "" || RUPass2.Password == "حشسسصخقي"))
            {
                RUPass1.Background = Brushes.Red;
                RUPass2.Background = Brushes.Red;
                return;
            }
            else
            {
                RUPass2.Background = Brushes.White;
            }
            if (RUPass1.Password == RUPass2.Password)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("accounts").Insert("Username", RUName.Text).Insert("Password", Kernel.Core.GetSHAHashData(RUPass1.Password)).Insert("State", state).Insert("Phone", RUPhone.Text).Execute();
                    IsRegisCom = true;
                    Kernel.Core.NoAccount = false;
                    if (Kernel.Core.IsSetup != true)
                    {
                        MessageBox.Show("تم تسجيل الموظف بنجاح");
                    }
                    Console.WriteLine("You add " + RUName.Text + " as " + state + " so much wow");
                }
                catch
                {
                    MessageBox.Show("المستخدم موجود من قبل");
                }
            }
            else
            {
                MessageBox.Show("كلمتى المرور غير متطابقتان");
            }
        }

        #region buttons status
        private void RUName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RUName.Text != "")
            {
                RUName.Background = Brushes.White;
            }
        }

        private void RUPass1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RUPass1.Password == "حشسسصخقي")
            {
                RUPass1.Password = "";
            }
        }

        private void RUPass1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RUPass1.Password == "")
            {
                RUPass1.Password = "حشسسصخقي";
            }
        }

        private void RUPass2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RUPass2.Password == "حشسسصخقي")
            {
                RUPass2.Password = "";
            }
        }

        private void RUPass2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RUPass2.Password == "")
            {
                RUPass2.Password = "حشسسصخقي";
            }
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Kernel.Core.IsSetup == true)
            {
                RUState.SelectedIndex = 0;
            }
        }

    }
}
