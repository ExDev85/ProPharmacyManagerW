// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using ProPharmacyManagerW.Kernel;
using System.Windows;

namespace ProPharmacyManagerW.Database
{
    public class MySqlReader
    {
        private MySqlConnection _conn = DataHolder.MySqlConnection;
        private DataRow _datarow;
        private DataSet _dataset;
        private int _row;
        const string Table = "table";

        public MySqlReader(MySqlCommand command)
        {
            if (command.Type == MySqlCommandType.SELECT)
            {
                TryFill(command);
            }
        }
        private MySqlConnection SelectConnection()
        {
            return DataHolder.MySqlConnection;
        }
        private string _lasterror = null;
        public string LastError
        {
            get
            {
                return _lasterror;
            }
            set
            {
                _lasterror = value;
            }
        }
        private void TryFill(MySqlCommand command)
        {
            MySqlConnection connection = SelectConnection();
            MySqlDataAdapter DataAdapter = null;
            if (connection.State == ConnectionState.Open)
            {
                while (_dataset == null && (_lasterror == null || _lasterror.Contains("connection")))
                {
                    if (_lasterror != null && _lasterror.Contains("connection"))
                        connection = SelectConnection();
                    DataAdapter = new MySqlDataAdapter(command.Command, connection);
                    _dataset = new DataSet();
                    try
                    {
                        DataAdapter.Fill(_dataset, Table);
                    }
                    catch (MySqlException e)
                    {
                        _lasterror = e.ToString().ToLower();
                        _dataset = null;
                        continue;
                    }
                    catch (Exception e)
                    {
                        Core.SaveException(e);
                        break;
                    }
                    _row = 0;
                }
            }
            using (MySqlConnection conn = SelectConnection())
            {
                try
                {
                    conn.Open();
                    DataAdapter = new MySqlDataAdapter(command.Command, connection);
                    _dataset = new DataSet();
                    try
                    {
                        DataAdapter.Fill(_dataset, Table);
                    }
                    catch (MySqlException e)
                    {
                        _lasterror = e.ToString().ToLower();
                        _dataset = null;
                    }
                    catch (Exception e)
                    {
                        Core.SaveException(e);
                    }
                    _row = 0;
                    conn.Close();
                }
                catch
                {
                    MessageBox.Show("هناك مشكله فى اتصالك بقاعده البيانات اعد تشغيل البرنامج لتتمكن من تعديل الاعدادات\n او اضغط f11");
                    File.Delete(Constants.SetupConfigPath);
                }
            }
        }
        public bool Read()
        {
            if (_dataset.Tables[Table].Rows.Count > _row)
            {
                _datarow = _dataset.Tables[Table].Rows[_row];
                _row++;
                return true;
            }
            _row++;
            return false;
        }
        public void Dispose()
        {
            if (_conn.State != ConnectionState.Closed)
                _conn.Close();
            _conn.Dispose();
            //Reader = null;
            _conn = null;
        }
        public void Close()
        {
            Dispose();
        }
        public sbyte ReadSByte(string columnName)
        {
            sbyte result = 0;
            sbyte.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public byte ReadByte(string columnName)
        {
            byte result = 0;
            byte.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public short ReadInt16(string columnName)
        {
            short result = 0;
            short.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public ushort ReadUInt16(string columnName)
        {
            ushort result = 0;
            ushort.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public int ReadInt32(string columnName)
        {
            int result = 0;
            int.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public uint ReadUInt32(string columnName)
        {
            uint result = 0;
            uint.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public long ReadInt64(string columnName)
        {
            long result = 0;
            long.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public ulong ReadUInt64(string columnName)
        {
            ulong result = 0;
            ulong.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public decimal ReadDecimal(string columnName)
        {
            decimal result = 0;
            decimal.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }
        public string ReadString(string columnName)
        {
            string result = "";
            result = _datarow[columnName].ToString();
            return result;
        }
        public bool ReadBoolean(string columnName)
        {
            bool result = false;
            string str = _datarow[columnName].ToString();
            if (str[0] == '1') return true;
            if (str[0] == '0') return false;

            bool.TryParse(_datarow[columnName].ToString(), out result);
            return result;
        }

    }
}