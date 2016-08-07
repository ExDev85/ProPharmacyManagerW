// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using MySql.Data.MySqlClient;

namespace ProPharmacyManagerW.Database
{
    public static class DataHolder
    {
        private static string MySqlUsername, MySqlPassword, MySqlDatabase, MySqlHost;

        private static string ConnectionString;

        /// <summary> 
        /// get connection information to connect to the database
        /// </summary> 
        /// <param name="user">database user</param>
        /// <param name="password">database password</param>
        /// <param name="database">database name</param>
        /// <param name="host">host (localhost)</param>
        public static void CreateConnection(string user, string password, string database, string host)
        {
            MySqlUsername = user;
            MySqlHost = host;
            MySqlPassword = password;
            MySqlDatabase = database;
            ConnectionString = "Server=" + MySqlHost + ";Database='" + MySqlDatabase + "';Username='" + MySqlUsername + "';Password='" + MySqlPassword + "';Pooling=true; Max Pool Size = 160000; Min Pool Size = 0;CHARSET=utf8";
        }

        /// <summary> 
        /// get connection information to create DB
        /// </summary> 
        /// <param name="user">database user</param>
        /// <param name="password">database password</param>
        /// <param name="host">host (localhost)</param>
        public static void CreateConnection(string user, string password, string host)
        {
            MySqlUsername = user;
            MySqlHost = host;
            MySqlPassword = password;
            ConnectionString = "Server=" + MySqlHost + ";Username='" + MySqlUsername + "';Password='" + MySqlPassword + "';Pooling=true; Max Pool Size = 160000; Min Pool Size = 0;CHARSET=utf8";
        }

        public static MySqlConnection MySqlConnection
        {
            get
            {
                MySqlConnection conn = new MySqlConnection();
                conn.ConnectionString = ConnectionString;
                return conn;
            }
        }

    }
}
