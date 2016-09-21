// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Windows;

namespace ProPharmacyManagerW.Database
{
    public class AccountsTable
    {
        public static string UserName;
        public static string UserPassword;

        private static States.AccountState State;
        /// <summary> 
        /// saves who and when he logged in
        /// </summary> 
        private static void SaveLogin()
        {
            if (Kernel.Core.aa == "1")
            {
                try
                {
                    MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd1.Insert("logs")
                        .Insert("Username", UserName).Insert("LoginDate", DateTime.Now.ToString()).Insert("Online", 1).Execute();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        /// <summary>
        /// Check user state
        /// </summary>
        /// <returns>true if admin</returns>
        public static bool IsAdmin()
        {
            if (State == States.AccountState.Manager)
            {
                return true;
            }
            else if (State == States.AccountState.Employee)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 
        /// login check code for user and password
        /// then return true value if it's correct
        /// show the right control panel and save logs
        /// </summary> 
        public static bool UserLogin()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("accounts").Where("Username", UserName).And("Password", UserPassword);
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    State = (States.AccountState)r.ReadByte("State");
                    switch (State)
                    {
                        case States.AccountState.Manager:
                            {
                                SaveLogin();
                                Console.WriteLine(UserName + " login as admin ha, i should go back to work then");
                                return true;
                            }
                        case States.AccountState.Employee:
                            {
                                SaveLogin();
                                Console.WriteLine(UserName + " what's up fellow worker");
                                return true;
                            }
                        default:
                            MessageBox.Show("من انت؟ هل تعمل فى هذه الصيدليه؟");
                            Console.WriteLine("User states is unknown");
                            return false;
                    }
                }
                else
                {
                    MessageBox.Show("أسم المستخدم و/او كلمه المرور خطأ");
                    return false;
                }
            }
            catch (Exception ll)
            {
                Console.WriteLine("Error while loging in");
                Kernel.Core.SaveException(ll);
                return false;
            }
        }

    }
}