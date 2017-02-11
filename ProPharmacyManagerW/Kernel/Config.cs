// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;

namespace ProPharmacyManagerW.Kernel
{
    public class Config
    {
        private string _hostName;
        private string _dbName;
        private string _dbUserName;
        private string _dbPassword;
        private string _accountsLog;
        private string _drugsLog;

        public string Hostname
        {
            get { return this._hostName; }
            set
            {
                if (value == "")
                {
                    value = "localhost";
                }
                this._hostName = value;
            }
        }

        public string DbName
        {
            get { return this._dbName; }
            set
            {
                if (value == "")
                {
                    value = "phdb";
                }
                this._dbName = value;
            }
        }

        public string DbUserName
        {
            get { return this._dbUserName; }
            set
            {
                if (value == "")
                {
                    value = "root";
                }
                this._dbUserName = value;
            }
        }

        public string DbUserPassword
        {
            get { return this._dbPassword; }
            set
            {
                if (value == "")
                {
                    value = "1234";
                }
                else if (value == "null")
                {
                    value = "";
                }
                this._dbPassword = value;
            }
        }

        public string Version { get; set; }

        public string AccountsLog
        {
            get { return this._accountsLog; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "1";
                }
                this._accountsLog = value;
            }
        }

        public string DrugsLog
        {
            get { return this._drugsLog; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "1";
                }
                this._drugsLog = value;
            }
        }

        IniFile file = new IniFile(Paths.SetupConfigPath);

        public Config()
        {
            if (string.IsNullOrEmpty(Version))
            {
                Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "");
            }
            if (string.IsNullOrEmpty(AccountsLog))
            {
                AccountsLog = "1";
            }
            if (string.IsNullOrEmpty(DrugsLog))
            {
                DrugsLog = "1";
            }
        }

        /// <summary>
        /// Read database info
        /// </summary>
        public void Read()
        {
            Hostname = Core.INIDecrypt(file.ReadString("MySql", "Host"));
            DbName = Core.INIDecrypt(file.ReadString("MySql", "Database"));
            DbUserName = Core.INIDecrypt(file.ReadString("MySql", "Username"));
            DbUserPassword = Core.INIDecrypt(file.ReadString("MySql", "Password"));
        }

        /// <summary>
        /// Read database info
        /// </summary>
        /// <param name="version">Read version info</param>
        public void Read(bool version)
        {
            Read();
            if (version)
            {
                Version = Core.INIDecrypt(file.ReadString("Upgrade", "Version"));
            }
        }

        /// <summary>
        /// Read database info
        /// </summary>
        /// <param name="version">Read version info</param>
        /// <param name="settings">Read accounts drugs logs</param>
        public void Read(bool version, bool settings)
        {
            Read(version);
            if (settings)
            {
                AccountsLog = Core.INIDecrypt(file.ReadString("Settings", "AccountsLog"));
                DrugsLog = Core.INIDecrypt(file.ReadString("Settings", "DrugsLog"));
            }
        }

        /// <summary>
        /// Read database info
        /// </summary>
        /// <param name="version">Read version info</param>
        /// <param name="accountsLog">Read accounts log</param>
        /// <param name="drugsLog">Read drugs log</param>
        public void Read(bool version, bool accountsLog, bool drugsLog)
        {
            Read(version);
            if (accountsLog)
            {
                AccountsLog = Core.INIDecrypt(file.ReadString("Settings", "AccountsLog"));
            }
            if (drugsLog)
            {
                DrugsLog = Core.INIDecrypt(file.ReadString("Settings", "DrugsLog"));
            }
        }

        /// <summary>
        /// Choose to read database,version,accounts Log and drugs Log info
        /// </summary>
        /// <param name="config"></param>
        /// <param name="version">Read version info</param>
        /// <param name="accountsLog">Read accounts log</param>
        /// <param name="drugsLog">Read drugs log</param>
        public void Read(bool config, bool version, bool accountsLog, bool drugsLog)
        {
            if (config)
            {
                Read();
            }
            if (version)
            {
                Version = Core.INIDecrypt(file.ReadString("Upgrade", "Version"));
            }
            if (accountsLog)
            {
                AccountsLog = Core.INIDecrypt(file.ReadString("Settings", "AccountsLog"));
            }
            if (drugsLog)
            {
                DrugsLog = Core.INIDecrypt(file.ReadString("Settings", "DrugsLog"));
            }
        }

        /// <summary>
        /// Write database info
        /// </summary>
        /// </summary>
        public void Write()
        {
            try
            {
                file.Write("MySql", "Host", Hostname);
                file.Write("MySql", "Database", DbName);
                file.Write("MySql", "Username", DbUserName);
                file.Write("MySql", "Password", DbUserPassword);
            }
            catch (Exception e)
            {
                Core.SaveException(e);
            }
        }

        /// <summary>
        /// Write database info
        /// </summary>
        /// <param name="version">Write version info</param>
        public void Write(bool version)
        {
            Write();
            if (version)
            {
                file.Write("Upgrade", "Version",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            }
        }

        /// <summary>
        /// Write database info
        /// </summary>
        /// <param name="version">Write version info</param>
        /// <param name="settings">Write accounts drugs logs</param>
        public void Write(bool version, bool settings)
        {
            Write(version);
            if (settings)
            {
                file.Write("Settings", "AccountsLog", AccountsLog);
                file.Write("Settings", "DrugsLog", DrugsLog);
            }
        }

        /// <summary>
        /// Write database info
        /// </summary>
        /// <param name="version">Write version info</param>
        /// <param name="accountsLog">Write accounts log</param>
        /// <param name="drugsLog">Write drugs log</param>
        public void Write(bool version, bool accountsLog, bool drugsLog)
        {
            Write(version);
            if (accountsLog)
            {
                file.Write("Settings", "AccountsLog", AccountsLog);
            }
            if (drugsLog)
            {
                file.Write("Settings", "DrugsLog", DrugsLog);
            }
        }

        /// <summary>
        /// Choose to write database,version,accounts Log and drugs Log info
        /// </summary>
        /// <param name="config"></param>
        /// <param name="version">Write version info</param>
        /// <param name="accountsLog">Write accounts log</param>
        /// <param name="drugsLog">Write drugs log</param>
        public void Write(bool config, bool version, bool accountsLog, bool drugsLog)
        {
            if (config)
            {
                Write();
            }
            if (version)
            {
                file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            }
            if (accountsLog)
            {
                file.Write("Settings", "AccountsLog", AccountsLog);
            }
            if (drugsLog)
            {
                file.Write("Settings", "DrugsLog", DrugsLog);
            }
        }
    }
}
