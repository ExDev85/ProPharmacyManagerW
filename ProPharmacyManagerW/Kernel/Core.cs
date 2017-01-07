// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ProPharmacyManagerW.Kernel
{
    public class Core
    {
        /// <summary>
        /// To check if the user setup the program for the first time
        /// </summary>
        public static bool IsSetup;
        /// <summary>
        /// To check if the user is going to upgrade
        /// </summary>
        public static bool IsUpgrading;
        /// <summary>
        /// Check if console is active or not
        /// </summary>
        public static bool IsCMode;
        /// <summary>
        /// The used passwords to encrypt the config file content and read it
        /// </summary>
        static readonly string PasswordHash = "SHC@HAM&ABRZ";
        static readonly string SaltKey = "P@FAMK!TRP";
        static readonly string VIKey = "@Gjg9!b8tf&T6jl4k1b";

        /// <summary>
        /// Automatic backing up is active
        /// </summary>
        public static string sb;
        /// <summary>
        /// Backingup type - 1 for daily - 2 for weekly - 3 for monthly
        /// </summary>
        public static string st;
        /// <summary>
        /// Backingup time or day
        /// </summary>
        public static string stt;
        /// <summary>
        /// Backingup date
        /// </summary>
        public static string std;
        /// <summary>
        /// Check if the backup for current datetime is taken
        /// </summary>
        static string tb;

        /// <summary> 
        /// The first thing that program is going to do after showing up
        /// like checking for database connection etc
        /// </summary> 
        public static void StartUp_Engine()
        {
            try
            {
                IniFile file2 = new IniFile(Paths.BackupConfigPath);
                if (!File.Exists(Paths.SetupConfigPath))
                {
                    IsSetup = true;
                    Set set = new Set { Title = "تنصيب البرنامج" };
                    set.ShowDialog();
                }
                Config co = new Config();
                co.Read(true);
                DataHolder.CreateConnection(co.DbUserName, co.DbUserPassword, co.DbName, co.Hostname);
                string ver = co.Version;
                if (Convert.ToInt16(ver) < Convert.ToInt16(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "")))
                {
                    IsSetup = true;
                    //IsUpgrading = true;
                    Set set = new Set { Title = "ترقية البرنامج" };
                    set.ShowDialog();
                }
                if (!IsSetup)
                {
                    co.Read(true, true);
                    BillsTable.LBN();
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmd.Update("logs").Set("Online", 0).Where("Online", 1).Execute();
                    sb = INIDecrypt(file2.ReadString("Settings", "Backup"));
                    tb = INIDecrypt(file2.ReadString("Settings", "TakeBackup"));
                    st = INIDecrypt(file2.ReadString("Settings", "Type"));
                    stt = INIDecrypt(file2.ReadString("Settings", "Time"));
                    std = INIDecrypt(file2.ReadString("Settings", "Date"));
                    #region database logs
                    if (co.AccountsLog == "0")
                    {
                        View.Pages.Settings.IsRecAcc = false;
                    }
                    else if (co.AccountsLog == "1")
                    {
                        View.Pages.Settings.IsRecAcc = true;
                    }
                    if (co.DrugsLog == "0")
                    {
                        View.Pages.Settings.IsRecMed = false;
                    }
                    else if (co.DrugsLog == "1")
                    {
                        View.Pages.Settings.IsRecMed = true;
                    }
                    #endregion
                    #region automatic backup system
                    if (File.Exists(Paths.BackupConfigPath))
                    {
                        try
                        {
                            if (Convert.ToByte(sb) == 1)
                            {
                                if (Convert.ToByte(st) == 1)
                                {
                                    string[] time = stt.Split(':');
                                    if (Convert.ToByte(tb) == 0)
                                    {
                                        if (DateTime.Now.Date == Convert.ToDateTime(std))
                                        {
                                            if (DateTime.Now.Hour - Convert.ToByte(time[0]) == 0 && DateTime.Now.Minute - Convert.ToByte(time[1]) >= 0)
                                            {
                                                BackUp.Backup("PROPHMW");
                                                file2.Write("Settings", "TakeBackup", "1");
                                                tb = "1";
                                                Console.WriteLine("Daily backup has been taken");
                                            }
                                            if (DateTime.Now.Hour - Convert.ToByte(time[0]) >= 1)
                                            {
                                                BackUp.Backup("PROPHMW");
                                                file2.Write("Settings", "TakeBackup", "1");
                                                tb = "1";
                                                Console.WriteLine("Daily backup has been taken");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (DateTime.Now.Date > Convert.ToDateTime(std))
                                        {
                                            file2.Write("Settings", "TakeBackup", "0");
                                            tb = "0";
                                            Console.WriteLine("Daily backup has been reset");
                                        }
                                    }
                                }
                                else if (Convert.ToByte(st) == 2)
                                {
                                    if (Convert.ToByte(tb) == 0)
                                    {
                                        if (Convert.ToByte(stt) <= Convert.ToByte(DateTime.Now.DayOfWeek))
                                        {
                                            BackUp.Backup("PROPHMW");
                                            file2.Write("Settings", "TakeBackup", "1");
                                            tb = "1";
                                            Console.WriteLine("Weekly backup has been taken");
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToByte(stt) > Convert.ToByte(DateTime.Now.DayOfWeek))
                                        {
                                            file2.Write("Settings", "TakeBackup", "0");
                                            tb = "0";
                                            Console.WriteLine("Weekly backup has been reset");
                                        }
                                    }
                                }
                                else if (Convert.ToByte(st) == 3)
                                {
                                    if (Convert.ToByte(tb) == 0)
                                    {
                                        if (Convert.ToByte(stt) <= Convert.ToByte(DateTime.Now.Day))
                                        {
                                            BackUp.Backup("PROPHMW");
                                            file2.Write("Settings", "TakeBackup", "1");
                                            tb = "1";
                                            Console.WriteLine("Monthly backup has been taken");
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToByte(stt) > Convert.ToByte(DateTime.Now.Day))
                                        {
                                            file2.Write("Settings", "TakeBackup", "0");
                                            tb = "0";
                                            Console.WriteLine("Monthly backup has been reset");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Check the automatic backup system in the core");
                            SaveException(e);
                            File.Delete(Paths.BackupConfigPath);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                SaveException(ex);
                if (File.Exists(Paths.SetupConfigPath))
                {
                    File.Delete(Paths.SetupConfigPath);
                }
                if (File.Exists(Paths.BackupConfigPath))
                {
                    File.Delete(Paths.BackupConfigPath);
                }
                Console.WriteLine("Program configuration files has been deleted \r\n now we are goning to shutdown your PC");
            }
        }
        /// <summary> 
        /// Save Exceptions to the program folder and write the error to Console
        /// </summary> 
        /// <param name="e">exception string</param>
        public static void SaveException(Exception e)
        {
            Console.WriteLine(e.ToString());
            if (e.TargetSite.Name == "ThrowInvalidOperationException") return;
            DateTime now = DateTime.Now;
            string str = string.Concat(new object[] { now.Month, "-", now.Day, "//" });
            if (!Directory.Exists(Paths.UnhandledExceptionsPath))
            {
                Directory.CreateDirectory(Paths.UnhandledExceptionsPath);
            }
            if (!Directory.Exists(Paths.UnhandledExceptionsPath + str))
            {
                Directory.CreateDirectory(Paths.UnhandledExceptionsPath + str);
            }
            if (!Directory.Exists(Paths.UnhandledExceptionsPath + str + e.TargetSite.Name))
            {
                Directory.CreateDirectory(Paths.UnhandledExceptionsPath + str + e.TargetSite.Name);
            }
            File.WriteAllLines((Paths.UnhandledExceptionsPath + str + e.TargetSite.Name + "\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Ticks & 10L }) + ".txt", new List<string>
                {
                    "----Exception message----",
                    e.Message,
                    "----End of exception message----\r\n",
                    "----Stack trace----",
                    e.StackTrace,
                    "----End of stack trace----\r\n"
                }.ToArray());
        }

        /// <summary> 
        /// passwords hash
        /// </summary> 
        /// <param name="data">string password before hassing</param>
        /// <returns>string password after hashing</returns>
        public static string GetSHAHashData(string data)
        {
            SHA512 sha1 = SHA512.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }
        /// <summary> 
        /// ini info encryptor
        /// </summary> 
        /// <param name="DecryptedText">unencrypted string</param>
        /// <returns>encrypted string</returns>
        public static string INIEncrypt(string DecryptedText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(DecryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            byte[] cipherTextBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Dispose();
                    cryptoStream.Close();
                }
                memoryStream.Dispose();
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        /// <summary> 
        /// ini info decryptor
        /// </summary> 
        /// <param name="encryptedText">encrypted string that we want to decrypt </param>
        /// <returns>decrypted string</returns>
        public static string INIDecrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Dispose();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
