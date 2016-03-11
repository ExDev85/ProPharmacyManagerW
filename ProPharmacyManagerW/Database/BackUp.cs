// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows;

namespace ProPharmacyManager.Database
{
    public class BackUp
    {
        /// <summary> 
        /// backup database the name will be dd-mm-yyyy hh.mm.ss.ms.sql
        /// </summary> 
        /// <param name="EnK">Encryption key</param>
        public static void Backup(string EnK)
        {
            try
            {
                string DTime = string.Format("{0}-{1}-{2} {3}.{4}.{5}.{6}", DateTime.Now.Day,
                    DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour,
                    DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                string file = "BackUp\\PHDB " + DTime + ".sql";
                DirectoryInfo dir1 = new DirectoryInfo("BackUp\\");
                using (var conn = DataHolder.MySqlConnection)
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        if (!dir1.Exists)
                        {
                            dir1.Create();
                        }
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportInfo.EnableEncryption = true;
                        mb.ExportInfo.EncryptionPassword = EnK;
                        mb.ExportToFile(file);
                        conn.Dispose();
                        conn.Close();
                    }
                    MessageBox.Show("تم اخذ نسخه احتياطيه من قاعدة البيانات بنجاح");
                }
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                MessageBox.Show("حدث خطا اثناء اخذ نسخه احتياطيه من قاعده البيانات");
            }
        }
        /// <summary> 
        /// restore a backup to database
        /// </summary> 
        /// <param name="RBU">backup file path</param>
        /// <param name="Dek">Encryption key</param>
        public static void Restore(string RBU, string Dek)
        {
            try
            {
                DirectoryInfo dir1 = new DirectoryInfo("BackUp\\");
                using (var conn = DataHolder.MySqlConnection)
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        if (!dir1.Exists)
                        {
                            MessageBox.Show("مجلد النسخ الاحتياطيه غير موجود");
                            return;
                        }
                        if (!File.Exists(RBU))
                        {
                            MessageBox.Show("النسخة الاحتياطيه غير موجودة");
                            return;
                        }
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportInfo.EnableEncryption = true;
                        mb.ImportInfo.EncryptionPassword = Dek;
                        mb.ImportFromFile(RBU);
                        conn.Dispose();
                        conn.Close();
                    }
                    MessageBox.Show("تم استعاده النسخة الاحتياطيه بنجاح");
                }
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                MessageBox.Show("مفتاح التشفير غير صحيح");
            }
        }
        /// <summary> 
        /// backup tables content when user change the database name from setting
        /// </summary> 
        public static void NewDbBackup()
        {
            try
            {
                const string file = "BackUp\\PHDBOldDBBackup.sql";
                DirectoryInfo dir1 = new DirectoryInfo("BackUp\\");
                using (var conn = DataHolder.MySqlConnection)
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        if (!dir1.Exists)
                        {
                            dir1.Create();
                        }
                        if (!File.Exists(file))
                        {
                            FileInfo f = new FileInfo(file);
                            FileStream fs = f.Create();
                            fs.Close();
                        }
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportInfo.EnableEncryption = false;
                        mb.ExportInfo.EncryptionPassword = "PROFelixSHCO";
                        mb.ExportToFile(file);
                        conn.Dispose();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                MessageBox.Show("حدث خطا اثناء اخذ نسخه احتياطيه من قاعده البيانات");
            }
        }
        /// <summary> 
        /// restore tables content when user change the database name from setting
        /// </summary> 
        public static void NewDbRestore()
        {
            try
            {
                const string file = "BackUp\\PHDBOldDBBackup.sql";
                DirectoryInfo dir1 = new DirectoryInfo("BackUp\\");
                using (var conn = DataHolder.MySqlConnection)
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        if (!dir1.Exists)
                        {
                            MessageBox.Show("مجلد النسخ الاحتياطيه غير موجود");
                        }
                        else if (!File.Exists(file))
                        {
                            MessageBox.Show("النسخة الاحتياطيه غير موجودة");
                        }
                        else
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ImportInfo.EnableEncryption = true;
                            mb.ImportInfo.EncryptionPassword = "PROFelixSHCO";
                            mb.ImportFromFile(file);
                            conn.Dispose();
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
                MessageBox.Show("حدث خطا اثناء استعادة نسخه احتياطيه من قاعده البيانات");
            }
        }
    }
}