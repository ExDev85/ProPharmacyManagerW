// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
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

        public void Read()
        {
            Hostname = Core.INIDecrypt(file.ReadString("MySql", "Host"));
            DbName = Core.INIDecrypt(file.ReadString("MySql", "Database"));
            DbUserName = Core.INIDecrypt(file.ReadString("MySql", "Username"));
            DbUserPassword = Core.INIDecrypt(file.ReadString("MySql", "Password"));
        }

        public void Read(bool version)
        {
            Hostname = Core.INIDecrypt(file.ReadString("MySql", "Host"));
            DbName = Core.INIDecrypt(file.ReadString("MySql", "Database"));
            DbUserName = Core.INIDecrypt(file.ReadString("MySql", "Username"));
            DbUserPassword = Core.INIDecrypt(file.ReadString("MySql", "Password"));
            if (version)
            {
                Version = Core.INIDecrypt(file.ReadString("Upgrade", "Version"));
            }
        }

        public void Read(bool version, bool settings)
        {
            Hostname = Core.INIDecrypt(file.ReadString("MySql", "Host"));
            DbName = Core.INIDecrypt(file.ReadString("MySql", "Database"));
            DbUserName = Core.INIDecrypt(file.ReadString("MySql", "Username"));
            DbUserPassword = Core.INIDecrypt(file.ReadString("MySql", "Password"));
            if (version)
            {
                Version = Core.INIDecrypt(file.ReadString("Upgrade", "Version"));
            }
            if (settings)
            {
                AccountsLog = Core.INIDecrypt(file.ReadString("Settings", "AccountsLog"));
                DrugsLog = Core.INIDecrypt(file.ReadString("Settings", "DrugsLog"));
            }
        }

        public void Read(bool version, bool accountsLog, bool drugsLog)
        {
            Hostname = Core.INIDecrypt(file.ReadString("MySql", "Host"));
            DbName = Core.INIDecrypt(file.ReadString("MySql", "Database"));
            DbUserName = Core.INIDecrypt(file.ReadString("MySql", "Username"));
            DbUserPassword = Core.INIDecrypt(file.ReadString("MySql", "Password"));
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

        public void Read(bool config, bool version, bool accountsLog, bool drugsLog)
        {
            if (config)
            {
                Hostname = Core.INIDecrypt(file.ReadString("MySql", "Host"));
                DbName = Core.INIDecrypt(file.ReadString("MySql", "Database"));
                DbUserName = Core.INIDecrypt(file.ReadString("MySql", "Username"));
                DbUserPassword = Core.INIDecrypt(file.ReadString("MySql", "Password"));
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

        public void Write()
        {
            file.Write("MySql", "Host", Hostname);
            file.Write("MySql", "Database", DbName);
            file.Write("MySql", "Username", DbUserName);
            file.Write("MySql", "Password", DbUserPassword);
        }

        public void Write(bool version)
        {
            file.Write("MySql", "Host", Hostname);
            file.Write("MySql", "Database", DbName);
            file.Write("MySql", "Username", DbUserName);
            file.Write("MySql", "Password", DbUserPassword);
            if (version)
            {
                file.Write("Upgrade", "Version",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            }
        }

        public void Write(bool version, bool settings)
        {
            file.Write("MySql", "Host", Hostname);
            file.Write("MySql", "Database", DbName);
            file.Write("MySql", "Username", DbUserName);
            file.Write("MySql", "Password", DbUserPassword);
            if (version)
            {
                file.Write("Upgrade", "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            }
            if (settings)
            {
                file.Write("Settings", "AccountsLog", AccountsLog);
                file.Write("Settings", "DrugsLog", DrugsLog);
            }
        }

        public void Write(bool version, bool accountsLog, bool drugsLog)
        {
            file.Write("MySql", "Host", Hostname);
            file.Write("MySql", "Database", DbName);
            file.Write("MySql", "Username", DbUserName);
            file.Write("MySql", "Password", DbUserPassword);
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

        public void Write(bool config, bool version, bool accountsLog, bool drugsLog)
        {
            if (config)
            {
                file.Write("MySql", "Host", Hostname);
                file.Write("MySql", "Database", DbName);
                file.Write("MySql", "Username", DbUserName);
                file.Write("MySql", "Password", DbUserPassword);
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
