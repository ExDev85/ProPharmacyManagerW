// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;

namespace ProPharmacyManagerW.Kernel
{
    public class Paths
    {
        public static string UnhandledExceptionsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\Exceptions\\";
        public static string SetupConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\Configuration.ini";
        public static string BackupConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\PPHMW\\BackupConfig.ini";
        public static string BackupsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PPHMWB\\";
    }
}
