// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using MySql.Data.MySqlClient;
using ProPharmacyManagerW.Database;
using ProPharmacyManagerW.Kernel;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View.Pages
{
    /// <summary>
    /// Interaction logic for Upgrade.xaml
    /// </summary>
    public partial class Upgrade : Page
    {
        public Upgrade()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Make sure that upgrading process is finished
        /// </summary>
        public static bool IsUpgradeComp = false;

        private void ExitB_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void UpgradeB_Click(object sender, RoutedEventArgs e)
        {
            //TODO add upgrade codes for older versions
            pB.Visibility = Visibility.Visible;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                if (PPHMCB.IsChecked == true)
                {
                    if (PPHMLV.SelectedIndex >= -1 && PPHMLV.SelectedIndex <= 13)
                    {
                        MessageBox.Show("لا يمكن الترقية من هذا الاصدار بعد");
                        Console.WriteLine("Codes needed");
                    }
                    else if (PPHMLV.SelectedIndex == 14)
                    {
                        const string accountst =
                            "ALTER TABLE `accounts` CHANGE `Username` `Username` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL FIRST, CHANGE `Password` `Password` TEXT CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, CHANGE `State` `State` tinyint(5) UNSIGNED NOT NULL DEFAULT 0 AFTER `Password`, CHANGE `Phone` `Phone` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL AFTER `State`, DROP `LastCheck`;";
                        const string bills =
                            "ALTER TABLE `bills` CHANGE `Name` `ClientName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL AFTER `ID`, CHANGE `User` `Cashier` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL AFTER `ClientName`, CHANGE `Medic` `Medics` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL AFTER `Cashier`;";
                        const string logs =
                            "ALTER TABLE `logs` CHANGE `Account` `Username` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL FIRST, ADD `Online` tinyint(5) UNSIGNED NOT NULL DEFAULT 0 AFTER `LogoutDate`, MODIFY COLUMN `LoginDate` datetime NOT NULL AFTER `Username`, MODIFY COLUMN `LogoutDate` datetime NULL DEFAULT NULL AFTER `LoginDate`;";
                        const string medics =
                            "ALTER TABLE `medics` ADD `Barcode` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL AFTER `Name`, CHANGE `Count` `Total` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `Type`, ADD `BPrice` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `Total`, CHANGE `Price` `SPrice` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `BPrice`, CHANGE `Substance` `ScientificName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL AFTER `Barcode`, CHANGE `Expiry` `ExpirationDate` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0' AFTER `ScientificName`, CHANGE `Note` `Notes` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL AFTER `SPrice`, ADD COLUMN `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT FIRST ,DROP PRIMARY KEY, ADD PRIMARY KEY(`Id`, `Name`), ADD COLUMN `Supplier` varchar(50) NULL AFTER `ScientificName`;";
                        const string medlog =
                            "ALTER TABLE `medlog` CHANGE `Name` `MedName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL FIRST, CHANGE `Total` `TotalAmount` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `SellDate`, CHANGE `Cost` `TotalPrice` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `TotalAmount`, ADD `Cashier` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL AFTER `TotalPrice`;";
                        const string suppliers =
                            "CREATE TABLE `suppliers`(`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT , `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `Salesman` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `Phones` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL, `Notes` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL, PRIMARY KEY (`Id`), UNIQUE INDEX `Name` (`Name`) USING BTREE)ENGINE=InnoDB DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci ROW_FORMAT=Compact;";
                        try
                        {
                            using (var conn = DataHolder.MySqlConnection)
                            {
                                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                                {
                                    cmd.Connection = conn;
                                    conn.Open();
                                    CDfcts();
                                    QueryExpress.ExecuteScalarStr(cmd, accountst + bills + logs + medics + medlog + suppliers);
                                    conn.Dispose();
                                    conn.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Now you are gonne need to fix this problem manually ;p");
                            Core.SaveException(ex);
                            MessageBox.Show("هناك مشكله غالبا بسبب انك اختارت اصدار خاطئ\nاتصل بالمطور لحل المشكله");
                            Environment.Exit(0);
                        }
                        IsUpgradeComp = true;
                    }
                }
                else if (PPHMWCB.IsChecked == true)
                {
                    if (PPHMWLV.SelectedIndex == -1)
                    {
                        MessageBox.Show("اختر اصدار للترقية منه اولا");
                        return;
                    }
                    else if (PPHMWLV.SelectedIndex == 0)
                    {
                        //0.9.9.1
                        const string logs =
                            "ALTER TABLE `logs` CHANGE `Username` `Username` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL FIRST, MODIFY COLUMN `LoginDate` datetime NOT NULL AFTER `Username`, MODIFY COLUMN `LogoutDate` datetime NULL DEFAULT NULL AFTER `LoginDate`;";
                        const string medics =
                            "ALTER TABLE `medics` CHANGE `ActivePrinciple` `ScientificName` VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL, CHANGE `Total` `Total` decimal(15,2) UNSIGNED NOT NULL DEFAULT '0.00' AFTER `Type`, ADD `BPrice` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `Total`, CHANGE `Price` `SPrice` decimal(15,2) UNSIGNED NOT NULL DEFAULT '0.00' AFTER `BPrice`, ADD COLUMN `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT FIRST ,DROP PRIMARY KEY, ADD PRIMARY KEY(`Id`, `Name`), ADD COLUMN `Supplier` varchar(50) NULL AFTER `ScientificName`;";
                        const string medlog =
                            "ALTER TABLE `medlog` CHANGE `Cashier` `Cashier` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL AFTER `TotalPrice`, CHANGE `TotalAmount` `TotalAmount` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `SellDate`, CHANGE `TotalPrice` `TotalPrice` decimal(15,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `TotalAmount`;";
                        const string suppliers =
                            "CREATE TABLE `suppliers` (`Id`  int(10) UNSIGNED NOT NULL AUTO_INCREMENT ,`Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL , `Salesman` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL , `Phones` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,`Notes` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,PRIMARY KEY (`Id`), UNIQUE INDEX `Name` (`Name`) USING BTREE )ENGINE=InnoDB DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci ROW_FORMAT=Compact;";
                        try
                        {
                            using (var conn = DataHolder.MySqlConnection)
                            {
                                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                                {
                                    cmd.Connection = conn;
                                    conn.Open();
                                    CDfcts();
                                    QueryExpress.ExecuteScalarStr(cmd, logs + medics + medlog + suppliers);
                                    conn.Dispose();
                                    conn.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Now you are gonne need to fix this problem manually ;p");
                            Core.SaveException(ex);
                            MessageBox.Show("هناك مشكله غالبا بسبب انك اختارت اصدار خاطئ\nاتصل بالمطور لحل المشكله");
                            Environment.Exit(0);
                        }
                    }
                    else if (PPHMWLV.SelectedIndex == 1)
                    {
                        //0.9.9.5
                        const string logs =
                            "ALTER TABLE `logs` CHANGE `Username` `Username` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL FIRST, MODIFY COLUMN `LoginDate` datetime NOT NULL AFTER `Username`, MODIFY COLUMN `LogoutDate` datetime NULL DEFAULT NULL AFTER `LoginDate`;";
                        const string medics =
                            "ALTER TABLE `medics` ADD COLUMN `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT FIRST ,DROP PRIMARY KEY, ADD PRIMARY KEY(`Id`, `Name`), ADD COLUMN `Supplier` varchar(50) NULL AFTER `ScientificName`;";
                        const string suppliers =
                            "CREATE TABLE `suppliers` (`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT ,`Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL , `Salesman` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL , `Phones` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,`Notes` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,PRIMARY KEY (`Id`), UNIQUE INDEX `Name` (`Name`) USING BTREE)ENGINE=InnoDB DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci ROW_FORMAT=Compact;";
                        try
                        {
                            using (var conn = DataHolder.MySqlConnection)
                            {
                                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                                {
                                    cmd.Connection = conn;
                                    conn.Open();
                                    CDfcts();
                                    QueryExpress.ExecuteScalarStr(cmd, logs + medics + suppliers);
                                    conn.Dispose();
                                    conn.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Now you are gonne need to fix this problem manually ;p");
                            Core.SaveException(ex);
                            MessageBox.Show("هناك مشكله غالبا بسبب انك اختارت اصدار خاطئ\nاتصل بالمطور لحل المشكله");
                            Environment.Exit(0);
                        }
                    }
                    else if (PPHMWLV.SelectedIndex == 2)
                    {
                        //0.9.9.6
                        const string logs =
                            "ALTER TABLE `logs` CHANGE `Username` `Username` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL FIRST, MODIFY COLUMN `LoginDate` datetime NOT NULL AFTER `Username`, MODIFY COLUMN `LogoutDate` datetime NULL DEFAULT NULL AFTER `LoginDate`;";
                        const string medics =
                            "ALTER TABLE `medics` ADD COLUMN `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT FIRST ,DROP PRIMARY KEY, ADD PRIMARY KEY(`Id`, `Name`), ADD COLUMN `Supplier` varchar(50) NULL AFTER `ScientificName`;";
                        const string suppliers =
                            "CREATE TABLE `suppliers` (`Id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT ,`Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL , `Salesman` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL , `Phones` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,`Notes` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL ,PRIMARY KEY (`Id`), UNIQUE INDEX `Name` (`Name`) USING BTREE )ENGINE=InnoDB DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci ROW_FORMAT=Compact;";
                        try
                        {
                            using (var conn = DataHolder.MySqlConnection)
                            {
                                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                                {
                                    cmd.Connection = conn;
                                    conn.Open();
                                    CDfcts();
                                    QueryExpress.ExecuteScalarStr(cmd, medics + logs + suppliers);
                                    conn.Dispose();
                                    conn.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Now you are gonne need to fix this problem manually ;p");
                            Core.SaveException(ex);
                            MessageBox.Show("هناك مشكله غالبا بسبب انك اختارت اصدار خاطئ\nاتصل بالمطور لحل المشكله" +ex.ToString());
                            Environment.Exit(0);
                        }
                    }
                    IsUpgradeComp = true;
                }
                else if (PPHMWXCB.IsChecked == true)
                {
                    if (PPHMLV.SelectedIndex >= -1 || PPHMLV.SelectedIndex <= 1)
                    {
                        MessageBox.Show("لا يمكن الترقية من هذا الاصدار بعد");
                    }
                }
                IniFile file = new IniFile(Paths.SetupConfigPath);
                file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                pB.Visibility = Visibility.Collapsed;
            });
        }

        private void WConB_Click(object sender, RoutedEventArgs e)
        {
            Config co = new Config();
            co.Write();
            Console.WriteLine("Config file has been written");
            Console.WriteLine("I see a little uninstaller in you");
            MessageBox.Show("تمت كتابه ملف الاعدادت بنجاح");
            IsUpgradeComp = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pB.Visibility = Visibility.Collapsed;
            if (Environment.OSVersion.Version.Build <= 2600)
            {
                pB.Height = 10;
            }
        }

        /// <summary>
        /// convert C# datetime type and data to mysql for V0.9.9.7
        /// </summary>
        void CDfcts()
        {
            var ncmd = new Database.MySqlCommand(MySqlCommandType.SELECT).Select("logs");
            MySqlReader r = new MySqlReader(ncmd);
            while (r.Read())
            {
                var uN = r.ReadString("Username");
                var uLi = r.ReadString("LoginDate");
                var uLo = r.ReadString("LogoutDate");
                var oUli = uLi;
                var dt1 = Convert.ToDateTime(uLi);
                uLi = dt1.ToString("yyyy-MM-dd HH:mm:ss");
                var ucmd = new Database.MySqlCommand(MySqlCommandType.UPDATE);
                if (uLo != "")
                {
                    var dt2 = Convert.ToDateTime(uLo);
                    uLo = dt2.ToString("yyyy-MM-dd HH:mm:ss");
                    ucmd.Update("logs")
                        .Set("LoginDate", uLi)
                        .Set("LogoutDate", uLo)
                        .Where("Username", uN)
                        .And("LoginDate", oUli)
                        .Execute();
                }
                else
                {
                    ucmd.Update("logs")
                        .Set("LoginDate", uLi)
                        .Where("Username", uN)
                        .And("LoginDate", oUli)
                        .Execute();
                }
            }
            r.Close();
        }
    }
}
