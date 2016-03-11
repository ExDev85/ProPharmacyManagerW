// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Text;

namespace ProPharmacyManager.Database
{
    public class BillsTable
    {
        public static ulong BillNO = 0;
        public static string bMName;
        public static decimal bMAmount = 0;
        public static decimal bMCost = 0;
        public static string bClient;
        private static string bMList;
        /// <summary> 
        /// get bill nubmer
        /// </summary> 
        public static void LBN()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Order("ID", true);
                MySqlReader r = new MySqlReader(cmd);
                BillNO = r.Read() ? r.ReadUInt32("ID") : 0;
                r.Close();
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
            }
        }
        /// <summary> 
        /// load a bill for user
        /// </summary> 
        private static void LoadBill()
        {
            try
            {
                MySqlCommand cmd =
                    new MySqlCommand(MySqlCommandType.SELECT).Select("bills").Where("ClientName", bClient).And("ID", BillNO);
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    bMList = r.ReadString("Medics");
                }
                r.Close();
            }
            catch (Exception ee)
            {
                Kernel.Core.SaveException(ee);
            }
        }
        /// <summary> 
        /// create a new bill
        /// </summary> 
        public static void newbill()
        {
            try
            {
                LoadBill();
                ++BillNO;
                StringBuilder Command1 = new StringBuilder();
                Command1.Append(bMAmount + "~");
                Command1.Append(bMName + "~");
                Command1.Append(bMCost + "#");
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("bills")
                    .Insert("ClientName", bClient)
                    .Insert("Cashier", AccountsTable.UserName)
                    .Insert("Medics", Command1.ToString())
                    .Insert("BillDate", DateTime.Now.ToString())
                    .Execute();
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
            }
        }
        /// <summary> 
        /// update exist bill when client name and bill number equal to database
        /// </summary> 
        public static void updatebill()
        {
            LoadBill();
            try
            {
                StringBuilder Command = new StringBuilder();
                Command.Append(bMAmount + "~");
                Command.Append(bMName + "~");
                Command.Append(bMCost + "#");
                bMList += Command;
                MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                Cmd.Update("bills")
                    .Set("Medics", bMList)
                    .Where("ID", BillNO).And("ClientName", bClient);
                Cmd.Execute();
            }
            catch (Exception e)
            {
                Kernel.Core.SaveException(e);
            }
        }
    }
}