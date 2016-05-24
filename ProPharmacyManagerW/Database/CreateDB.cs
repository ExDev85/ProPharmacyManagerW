// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using MySql.Data.MySqlClient;
using ProPharmacyManagerW.Kernel;
using System;

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
            string db1 = "DROP DATABASE IF EXISTS `" + dbname + "`;CREATE DATABASE `" + dbname + "`;";
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
                "CREATE TABLE `logs` (`Username` varchar(20) NOT NULL, `LoginDate` varchar(30) NOT NULL, `LogoutDate` varchar(30) default NULL, `Online` tinyint(5) unsigned NOT NULL default '0') ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            const string medics =
                "CREATE TABLE `medics` (`Name` varchar(50) NOT NULL, `Barcode` varchar(50) default NULL, `ScientificName` varchar(50) default NULL, `ExpirationDate` varchar(10) NOT NULL default '0', `Type` tinyint(5) unsigned NOT NULL default '0', `Total` decimal(15,2) unsigned NOT NULL default '0', `BPrice` decimal(15,2) unsigned NOT NULL default '0', `SPrice` decimal(15,2) unsigned NOT NULL default '0', `Notes` text, PRIMARY KEY  (`Name`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            const string medlog =
                "CREATE TABLE `medlog` (`MedName` varchar(50) NOT NULL, `SellDate` varchar(20) NOT NULL default '0', `TotalAmount` decimal(15,2) unsigned NOT NULL default '0', `TotalPrice` decimal(15,2) unsigned NOT NULL default '0', `Cashier` varchar(30) NOT NULL) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            const string suppliers =
                "CREATE TABLE `suppliers` (`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, `Name` varchar(50) NOT NULL, `Salesman` varchar(50) NOT NULL, `Phones` text, `Notes` text, PRIMARY KEY(`Id`), UNIQUE KEY `Name` (`Name`)) ENGINE = InnoDB DEFAULT CHARSET = utf8;";
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
        /// <summary> 
        /// upgrade tables after make changes to them
        /// </summary> 
        public static void UpgradeTables()
        {
            IniFile file = new IniFile(Paths.SetupConfigPath);
            //TODO add upgrade codes when needed
            if (Convert.ToInt16(Core.INIDecrypt(file.ReadString("Upgrade", "Version"))) > 0990 && Convert.ToInt16(Core.INIDecrypt(file.ReadString("Upgrade", "Version"))) < 0995)
            {
                const string medics =
                    "ALTER TABLE `medics` CHANGE `ActivePrinciple` `ScientificName` VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL, CHANGE `Total` `Total` DECIMAL(15,2) UNSIGNED NOT NULL DEFAULT '0', ADD `BPrice` decimal(15,2) unsigned NOT NULL default '0' AFTER `Total`, ALTER TABLE `medics` CHANGE `Price` `SPrice` DECIMAL(15,2) UNSIGNED NOT NULL DEFAULT '0';";
                const string medlog =
                    " ALTER TABLE `medlog` CHANGE `Cashier` `Cashier` VARCHAR(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, CHANGE `TotalAmount` `TotalAmount` DECIMAL(15,2) UNSIGNED NOT NULL DEFAULT '0', CHANGE `TotalPrice` `TotalPrice` DECIMAL(15,2) UNSIGNED NOT NULL DEFAULT '0';";
                const string suppliers =
                    "CREATE TABLE IF NOT EXISTS `suppliers` (`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, `Name` varchar(50) NOT NULL, `Salesman` varchar(50) NOT NULL, `Phones` text, `Notes` text, PRIMARY KEY(`Id`), UNIQUE KEY `Name` (`Name`)) ENGINE = InnoDB DEFAULT CHARSET = utf8;";
                using (var conn = DataHolder.MySqlConnection)
                {
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        QueryExpress.ExecuteScalarStr(cmd, medics + medlog + suppliers);
                        conn.Dispose();
                        conn.Close();
                    }
                }
            }
            else
            {
                //Upgrade from Normal version Version 5.1.1.0 to the new W one current Version
                const string accountst =
                    "ALTER TABLE `accounts` CHANGE `Username` `Username` VARCHAR(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, CHANGE `Password` `Password` TEXT CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, CHANGE `State` `State` TINYINT(5) UNSIGNED NOT NULL DEFAULT '0', CHANGE `Phone` `Phone` VARCHAR(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL, DROP `LastCheck`;";
                const string bills =
                    "ALTER TABLE `bills` CHANGE `Name` `ClientName` VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL, CHANGE `User` `Cashier` VARCHAR(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL, CHANGE `Medic` `Medics` TEXT CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL;";
                const string logs =
                    "ALTER TABLE `logs` CHANGE `Account` `Username` VARCHAR(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, ADD `Online` TINYINT(5) UNSIGNED NOT NULL DEFAULT '0';";
                const string medics =
                    "ALTER TABLE `medics` ADD `Barcode` VARCHAR(50) NULL AFTER `Name`, CHANGE `Count` `Total` decimal(15,2) unsigned NOT NULL default '0' AFTER `Type`, ADD `BPrice` decimal(15,2) unsigned NOT NULL default '0' AFTER `Total`, CHANGE `Price` `SPrice` decimal(15,2) unsigned NOT NULL default '0' AFTER `Total`, CHANGE `Substance` `ScientificName` VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL , CHANGE `Expiry` `ExpirationDate` VARCHAR(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0', CHANGE `Note` `Notes` TEXT NULL DEFAULT NULL;";
                const string medlog =
                    "ALTER TABLE `medlog` CHANGE `Name` `MedName` VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, CHANGE `Total` `TotalAmount` DECIMAL(15,2) UNSIGNED NOT NULL DEFAULT '0', CHANGE `Cost` `TotalPrice` DECIMAL(15,2) UNSIGNED NOT NULL DEFAULT '0',  ADD `Cashier` VARCHAR(30) NOT NULL AFTER `TotalPrice`;";
                const string suppliers =
                    "CREATE TABLE IF NOT EXISTS `suppliers` (`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, `Name` varchar(50) NOT NULL, `Salesman` varchar(50) NOT NULL, `Phones` text, `Notes` text, PRIMARY KEY(`Id`), UNIQUE KEY `Name` (`Name`)) ENGINE = InnoDB DEFAULT CHARSET = utf8;";
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
}