// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using MySql.Data.MySqlClient;
using ProPharmacyManagerW.Kernel;
using System;
using System.IO;
using System.Windows;

namespace ProPharmacyManagerW.Database
{
    public class CreateDB
    {
        /// <summary> 
        /// The database name that the program creates at first run
        /// </summary> 
        /// <param name="dbname">Database name</param> 
        public static void Createdb(string dbname)
        {
            Console.WriteLine("Creating the database");
            string db1 = "DROP DATABASE IF EXISTS `" + dbname + "`;CREATE DATABASE `" + dbname + "`;";
            using (var conn = DataHolder.MySqlConnection)
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    cmd.Connection = conn;
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something went wrong \"Microsoft style\"\n" + e);
                        MessageBox.Show("هناك مشكله فى السيرفر او بيانات الاتصال به");
                        Core.SaveException(e);
                        if (File.Exists(Paths.SetupConfigPath))
                        {
                            File.Delete(Paths.SetupConfigPath);
                        }
                        if (File.Exists(Paths.BackupConfigPath))
                        {
                            File.Delete(Paths.BackupConfigPath);
                        }
                        Console.WriteLine("Program configuration files has been deleted \r\n now we are goning to shutdown your PC");
                        Environment.Exit(0);
                    }
                    string db = QueryExpress.ExecuteScalarStr(cmd, db1);
                    if (string.IsNullOrEmpty(db))
                    {
                        return;
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
        }

        /// <summary> 
        /// when user change the database name from setting
        /// </summary> 
        /// <param name="odbname">old Database name</param> 
        /// <param name="ndbname">new Database name</param> 
        public static void Createdb(string odbname, string ndbname)
        {
            string db1 = "DROP DATABASE IF EXISTS `" + odbname + "`;CREATE DATABASE IF NOT EXISTS `" + ndbname + "`;";
            using (var conn = DataHolder.MySqlConnection)
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    string db = QueryExpress.ExecuteScalarStr(cmd, db1);
                    if (string.IsNullOrEmpty(db))
                    {
                        return;
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
        }

        /// <summary> 
        /// create tables
        /// </summary> 
        public static void CreateTables()
        {
            const string accountst =
                "CREATE TABLE `accounts` (`Username` varchar(30) NOT NULL, `Password` text NOT NULL, `State` tinyint(5) unsigned NOT NULL default '0', `Phone` varchar(20) default NULL, PRIMARY KEY  (`Username`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            const string bills =
                "CREATE TABLE `bills` (`ID` int(20) NOT NULL auto_increment, `ClientName` varchar(50) default NULL, `Cashier` varchar(30) default NULL, `Medics` text, `BillDate` varchar(100) default NULL, PRIMARY KEY  (`ID`)) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;";
            const string logs =
                "CREATE TABLE `logs` (`Username` varchar(20) NOT NULL, `LoginDate` datetime NOT NULL, `LogoutDate` datetime NULL, `Online` tinyint(5) unsigned NOT NULL default '0') ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            const string medics =
                "CREATE TABLE `medics` (`Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT, `Name` varchar(50) NOT NULL, `Barcode` varchar(50) DEFAULT NULL, `ScientificName` varchar(50) DEFAULT NULL, `Supplier` varchar(50) DEFAULT NULL, `ExpirationDate` varchar(10) NOT NULL DEFAULT '0', `Type` tinyint(5) unsigned NOT NULL DEFAULT '0', `Total` decimal(15,2) unsigned NOT NULL DEFAULT '0.00', `BPrice` decimal(15,2) unsigned NOT NULL DEFAULT '0.00', `SPrice` decimal(15,2) unsigned NOT NULL DEFAULT '0.00', `Notes` text, PRIMARY KEY (`Id`,`Name`)) ENGINE=InnoDB AUTO_INCREMENT=3803 DEFAULT CHARSET=utf8;";
            const string medlog =
                "CREATE TABLE `medlog` (`MedName` varchar(50) NOT NULL, `SellDate` varchar(20) NOT NULL default '0', `TotalAmount` decimal(15,2) unsigned NOT NULL default '0', `TotalPrice` decimal(15,2) unsigned NOT NULL default '0', `Cashier` varchar(30) NOT NULL) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            const string suppliers =
                "CREATE TABLE `suppliers` (`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, `Name` varchar(50) NOT NULL, `Salesman` varchar(50) NOT NULL, `Phones` text, `Notes` text, PRIMARY KEY(`Id`), UNIQUE KEY `Name` (`Name`)) ENGINE = InnoDB DEFAULT CHARSET = utf8;";
            Console.WriteLine("Creating Tables");
            using (var conn = DataHolder.MySqlConnection)
            {
                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    QueryExpress.ExecuteScalarStr(cmd, accountst + bills + logs + medics + medlog + suppliers);
                    conn.Dispose();
                    conn.Close();
                }
            }
        }

    }
}