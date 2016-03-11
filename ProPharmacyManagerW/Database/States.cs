// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
namespace ProPharmacyManager.Database
{
    internal class States
    {
        /// <summary> 
        /// user status
        /// </summary>
        public enum AccountState : byte
        {
            Employee = 1,
            Manager = 2,
            None = 0
        }
    }
}