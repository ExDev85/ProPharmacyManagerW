// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Database;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProPharmacyManager.Pages
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
        /// Check if registertion prograss compelete for setup process
        /// </summary>
        public static bool IsRegister;
        public static bool IsRegisCom;

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
            if (RUName.Text == "" || RUName.Text == "اسم المستخدم*")
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
                    if (RUPhone.Text == "رقم الهاتف")
                    {
                        RUPhone.Text = "";
                    }
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("accounts").Insert("Username", RUName.Text).Insert("Password", Kernel.Core.GetSHAHashData(RUPass1.Password)).Insert("Phone", RUPhone.Text).Insert("State", state).Execute();
                    MessageBox.Show("تم تسجيل المستخدم بنجاح");
                    IsRegisCom = true;
                    if (Kernel.Core.IsSetup == true)
                    {
                        IsRegister = true;
                    }
                    Console.WriteLine("You add " + RUName.Text + " as "  + state + " so much wow");
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
        private void RUName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RUName.Text == "اسم المستخدم*")
            {
                RUName.Text = "";
            }
        }

        private void RUName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RUName.Text == "")
            {
                RUName.Text = "اسم المستخدم*";
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

        private void RUPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RUPhone.Text == "رقم الهاتف")
            {
                RUPhone.Text = "";
            }
        }

        private void RUPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RUPhone.Text == "")
            {
                RUPhone.Text = "رقم الهاتف";
            }
        }
        #endregion

        private void RUName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RUName.Text != "" || RUName.Text != "اسم المستخدم*")
            {
                RUName.Background = Brushes.White;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Kernel.Core.IsSetup == true)
            {
                RUState.SelectedIndex = 1;
            }
            else
            {
                RUState.SelectedIndex = 0;
            }
        }
    }
}
