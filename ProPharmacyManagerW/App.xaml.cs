// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Security.Principal;
using System.Windows;

namespace ProPharmacyManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = identity != null ? new WindowsPrincipal(identity) : null;
            if (principal == null || !principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("يجب تشغيل البرنامج كمدير\nThis application requires Administrator privileges.","خطأ",MessageBoxButton.OK,MessageBoxImage.Warning);
                Environment.Exit(0);
            }
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            Kernel.Core.StartUp_Engine();
        }
    }
}
