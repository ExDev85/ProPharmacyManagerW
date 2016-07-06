// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.Kernel;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ProPharmacyManagerW
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
        /// <returns>now full time</returns>
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
        /// write string in the same line without time stamp
        /// </summary>
        /// <param name="value">the new text to add</param>
        /// <returns></returns>
        public static string WriteT(string value)
        {
            NewEntry = true;
            GS = value;
            GSLog += value;
            return GS;
        }
        /// <summary>
        /// write string then new line
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
        /// new line then write string
        /// </summary>
        /// <param name="value">the new text to add</param>
        /// <returns></returns>
        public static string WriteLineF(string value)
        {
            NewEntry = true;
            GS = "\n" + TimeStamp() + value + "\n";
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
                    IniFile file = new IniFile(Paths.SetupConfigPath);
                    switch (data[0])
                    {
                        //TODO add more and more and more commands
                        #region Add new account
                        case "#addacc":
                            {
                                try
                                {
                                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                                    cmd.Insert("accounts").Insert("Username", data[1]).Insert("Password", Core.GetSHAHashData(data[2])).Insert("State", data[3]).Insert("Phone", data[4]).Execute();
                                    WriteLine("You add a new user " + data[1]);
                                }
                                catch (Exception e)
                                {
                                    WriteLine("There is something wrong maybe the username is already used.");
                                    Core.SaveException(e);
                                }
                                break;
                            }
                        #endregion
                        #region Add new drug
                        case "#adddrug":
                            {
                                try
                                {
                                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                                    cmd.Insert("medics")
                                        .Insert("Name", data[1])
                                        .Insert("Barcode", data[2])
                                        .Insert("ScientificName", data[3])
                                        .Insert("ExpirationDate", data[4])
                                        .Insert("Type", data[5])
                                        .Insert("Total", Convert.ToDecimal(data[6]))
                                        .Insert("SPrice", Convert.ToDecimal(data[7]))
                                        .Insert("Notes", data[8]).Execute();
                                    WriteLine(AccountsTable.UserName + " add " + data[6] + " " + data[1] + " which each cost " + data[7]);
                                }
                                catch (Exception e)
                                {
                                    WriteLine("There is something wrong maybe the drug is already exist.");
                                    Core.SaveException(e);
                                }
                                break;
                            }
                        #endregion
                        #region Delete table or database
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
                                            File.Delete(Paths.SetupConfigPath);
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
                        #endregion
                        #region Insert medics to the table
                        case "#import":
                            {
                                try
                                {
                                    Thread th = new Thread(() =>
                                    {
                                        using (StreamReader sr = File.OpenText(data[1]))
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            while (sb.Append(sr.ReadLine()).Length > 0)
                                            {
                                                retry:
                                                try
                                                {
                                                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT)
                                                    {
                                                        Command = sb.ToString()
                                                    };
                                                    cmd.Execute();
                                                    WriteT(".");
                                                    sb.Clear();
                                                }
                                                catch
                                                {
                                                    sb.Replace("INSERT INTO `medics`", "INSERT IGNORE INTO `medics`");
                                                    goto retry;
                                                }
                                            }
                                            sr.Dispose();
                                            sr.Close();
                                        }
                                        WriteLineF("The file is well imported");
                                    });
                                    th.Start();
                                }
                                catch (Exception e)
                                {
                                    WriteLine("Are you 100% sure that is a MySQL file/n" + e);
                                }
                                break;
                            }
                        #endregion
                        #region Delete config folder
                        case "#deltemp":
                            {
                                if (data.Length >= 2)
                                {
                                    if (data[1] == "-all")
                                    {
                                        try
                                        {
                                            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW"))
                                            {
                                                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW");
                                                WriteLine("You just deleted everything the program stand for \nI hope you are happy");
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            WriteLine("Maybe the folder isn't there so stop trying to delete it");
                                            Core.SaveException(e);
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\BackupConfig.ini"))
                                        {
                                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\BackupConfig.ini");
                                        }
                                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\Configuration.ini"))
                                        {
                                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\Configuration.ini");
                                        }
                                        WriteLine("You just deleted the config files");
                                    }
                                    catch (Exception e)
                                    {
                                        WriteLine("Maybe the files are not there so stop trying to delete them");
                                        Core.SaveException(e);
                                    }
                                }
                                break;
                            }
                        #endregion
                        case "#help":
                            {
                                WriteLine(@"
        #addacc Username Password State(type 2 for admin - 1 for employee) PhoneNumber(could be empty -type null-)
        #adddrug Name Barcode(Could be empty -type null-) ScientificName(Could be empty -type null-) ExpirationDate(should be yyyy/mm/dd) Type(type 1 for syrup - 2 for tab - 3 Injection - 4 for Cream/Ointments - 0 for other) Total(must be numbers) SPrice(must be numbers) Notes(Could be empty -type null-)
        #Drop db (to delete your database good luck with that)
        #Drop table tablename (delete a spacific table to ruin the database)
        #import path (type the full path for the sql file to import it like c:\meds.sql)
        #deltemp (type '-all' to delete the config folder with backups files like #deltemp -all)");
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
