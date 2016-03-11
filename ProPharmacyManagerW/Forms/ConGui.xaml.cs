// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Windows;
using System.Windows.Threading;

namespace ProPharmacyManager.Forms
{
    /// <summary>
    /// Interaction logic for ConGui.xaml
    /// </summary>
    public partial class ConGui : Window
    {
        public ConGui()
        {
            InitializeComponent();
            Kernel.Core.IsCMode = true;

        }
        private DispatcherTimer checkInput = new DispatcherTimer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IOBox.Text += Console.GSLog;
            checkInput.Interval = TimeSpan.FromMilliseconds(100);
            checkInput.Tick += checkInputState;
            checkInput.Start();
        }

        private void checkInputState(object sender, EventArgs e)
        {
            if (Console.NewEntry == true)
            {
                IOBox.Text += Console.GS;
                IOBox.ScrollToEnd();
                Console.NewEntry = false;
            }
        }

        private void IOBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (IOBox.GetLineText(IOBox.GetLastVisibleLineIndex()).Length == 0 || IOBox.GetLineText(IOBox.GetLastVisibleLineIndex()).Contains("You must start your command with # like"))
                {
                    return;
                }
                else 
                {
                    Console.CommandsAI(IOBox.GetLineText(IOBox.GetLastVisibleLineIndex()));
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Kernel.Core.IsCMode = false;
        }
    }
}
