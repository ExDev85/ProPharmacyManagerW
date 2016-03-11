// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManager.Database;
using ProPharmacyManager.Kernel;
using System;
using System.IO;

namespace ProPharmacyManager
{
    public class Console
    {
        /// <summary>
        /// Current console entry
        /// </summary>
        public static string GS;
        /// <summary>
        /// Logs for all console entries when it's not active
        /// </summary>
        public static string GSLog;
        /// <summary>
        /// to see if program write anything to console
        /// </summary>
        public static bool NewEntry;
        /// <summary>
/// entry time
/// </summary>
/// <returns></returns>
        public static string TimeStamp()
        {
            DateTime NOW = DateTime.Now;
            return "[" + NOW.Hour + ":" + NOW.Minute + ":" + NOW.Second.ToString() + ":" + NOW.Millisecond.ToString() + "] ";
        }
        /// <summary>
        /// write string in the same line
        /// </summary>
        /// <param name="value">the new text to add</param>
        /// <returns></returns>
        public static string Write(string value)
        {
            NewEntry = true;
            GS = TimeStamp() + value;
            GSLog += TimeStamp() + value;
            return GS;
        }
        /// <summary>
        /// write string to new line
        /// </summary>
        /// <param name="value">the new text to add</param>
        /// <returns></returns>
        public static string WriteLine(string value)
        {
            NewEntry = true;
            GS = TimeStamp() + value + "\n";
            GSLog += TimeStamp() + value + "\n";
            return GS;
        }
        /// <summary>
        /// Console commands that makes user do stuff
        /// </summary>
        /// <param name="command">the command</param>
        public static void CommandsAI(string command)
        {
            if (command == null)
                return;
            GSLog += command;
            if (AccountsTable.IsAdmin() == true)
            {
                if (command.StartsWith("#"))
                {
                    string[] data = command.Split(' ');
                    IniFile file = new IniFile(Constants.SetupConfigPath);
                    switch (data[0])
                    {
                        //TODO add more and more and more commands
                        case "#addacc":
                            {
                                try
                                {
                                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                                    cmd.Insert("accounts").Insert("Username", data[1]).Insert("Password", Kernel.Core.GetSHAHashData(data[2])).Insert("State", data[3]).Insert("Phone", data[4]).Execute();
                                    WriteLine("You add a new user " + data[1]);
                                }
                                catch (Exception e)
                                {
                                    WriteLine("There is something wrong maybe the username is already used.");
                                    Kernel.Core.SaveException(e);
                                }
                                break;
                            }
                        case "#adddrug":
                            {
                                try
                                {
                                    //TODO this command needs more work
                                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                                    cmd.Insert("medics")
                                        .Insert("Name", data[1])
                                        .Insert("Barcode", data[2])
                                        .Insert("ActivePrinciple", data[3])
                                        .Insert("ExpirationDate", data[4])
                                        .Insert("Type", data[5])
                                        .Insert("Total", Convert.ToDecimal(data[6]))
                                        .Insert("Price", Convert.ToDecimal(data[7]))
                                        .Insert("Notes", data[8]).Execute();
                                    WriteLine(AccountsTable.UserName + " add " + data[6] + " " + data[1] + " which each cost " + data[7]);
                                }
                                catch (Exception e)
                                {
                                    WriteLine("There is something wrong maybe the drug is already exist.");
                                    Kernel.Core.SaveException(e);
                                }
                                break;
                            }
                        case "#Drop":
                            {
                                if (data[1] == "db")
                                {
                                    string Ddb = "DROP DATABASE IF EXISTS `" + Core.INIDecrypt(file.ReadString("MySql", "Database")) + "`;";
                                    using (var conn = DataHolder.MySqlConnection)
                                    {
                                        using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                                        {
                                            cmd.Connection = conn;
                                            conn.Open();
                                            string db = MySql.Data.MySqlClient.QueryExpress.ExecuteScalarStr(cmd, Ddb);
                                            if (string.IsNullOrEmpty(db))
                                            {
                                                return;
                                            }
                                            conn.Dispose();
                                            conn.Close();
                                            File.Delete(Constants.SetupConfigPath);
                                        }
                                    }
                                }
                                else if (data[1] == "table")
                                {
                                    string Dtab = "DROP TABLE `" + data[2] + "`;";
                                    try
                                    {
                                        using (var conn = DataHolder.MySqlConnection)
                                        {
                                            using (MySql.Data.MySqlClient.MySqlCommand mCmd = new MySql.Data.MySqlClient.MySqlCommand(Dtab, conn))
                                            {
                                                mCmd.ExecuteNonQuery();
                                            }
                                            conn.Clone();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        WriteLine("Maybe you entered a wrong table name");
                                        Core.SaveException(e);
                                    }
                                }
                                break;
                            }
                        case "#help":
                            {
                                WriteLine("#addacc Username Password State(type 2 for admin - 1 for employee) PhoneNumber(could be empty -type null-)\n#adddrug Name Barcode(Could be empty -type null-) ActivePrinciple(Could be empty -type null-) ExpirationDate(should be yyyy/mm/dd) Type(type 1 for syrup - 2 for tab - 3 Injection - 4 for Cream/Ointments - 0 for other) Total(must be numbers) Price(must be numbers) Notes(Could be empty -type null-)");
                                break;
                            }
                        default:
                            {
                                WriteLine("This command shall not execute");
                                break;
                            }
                    }
                }
                else
                {
                    WriteLine("You must start your command with # like #help .");
                }
            }
            else
            {
                WriteLine("You must login as admin");
            }
        }
    }
}
