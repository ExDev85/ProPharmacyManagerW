// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System.Windows;
using System.Windows.Input;

namespace ProPharmacyManagerW.Forms
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://creativecommons.org/licenses/by-nc-sa/4.0/");
        }

        private void label3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shababco.blogspot.com");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            verl.Content = "الاصدار: W " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
