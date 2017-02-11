// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProPharmacyManagerW.View
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
            richTextBox.AppendText("Welcome to PPHM Console window\rI really hope that you know what you are doing here!!\rYou can write commands in here see every step or even better erase the whole database have fun ;)\n|>");
            richTextBox.ScrollToEnd();
            richTextBox.CaretPosition = richTextBox.CaretPosition.DocumentEnd;
            try
            {
                if (Console.NewEntry == true)
                {
                    lock (Console.GSLog)
                    {
                        richTextBox.AppendText(Console.GSLog);
                        Console.GSLog = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Kernel.Core.SaveException(ex);
            }
            checkInput.Interval = TimeSpan.FromMilliseconds(1000);
            checkInput.Tick += checkInputState;
            checkInput.Start();
            richTextBox.Focus();
        }

        private void checkInputState(object sender, EventArgs e)
        {
            if (Console.NewEntry == true)
            {
                lock (Console.GSLog)
                {
                    richTextBox.AppendText(Console.GSLog);
                    Console.GSLog = "";
                    Console.GS = "";
                }
                richTextBox.ScrollToEnd();
                richTextBox.CaretPosition = richTextBox.CaretPosition.DocumentEnd;
                Console.NewEntry = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(Console.GS))
                {
                    lock (Console.GS)
                    {
                        richTextBox.AppendText(Console.GS);
                        Console.GS = "";
                    }
                    richTextBox.ScrollToEnd();
                    richTextBox.CaretPosition = richTextBox.CaretPosition.DocumentEnd;
                    Console.NewEntry = false;
                }
            }
            if (Console.IsProgressing)
            {
                lock (Console.progress.ToString())
                {
                    var currentLine = new TextRange(richTextBox.CaretPosition.GetLineStartPosition(0), richTextBox.CaretPosition.GetLineStartPosition(1) ?? richTextBox.CaretPosition.DocumentEnd).Text;
                    Regex reg1 = new Regex(@"\s\d+%");
                    MatchCollection fou1 = reg1.Matches(currentLine);
                    foreach (Match m in fou1)
                    {
                        if (m.Success)
                        {
                            currentLine = currentLine.Replace(m.Value, " " + Console.progress.ToString() + "%");
                            if (currentLine != new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text)
                            {
                                new TextRange(richTextBox.CaretPosition.GetLineStartPosition(0), richTextBox.CaretPosition.GetLineStartPosition(1) ?? richTextBox.CaretPosition.DocumentEnd).Text = currentLine;
                            }
                        }
                    }
                    if (Console.progress % 10 == 0 && Console.progress != 0)
                    {
                        Regex reg2 = new Regex(@"[[^#]-");
                        MatchCollection fou2 = reg2.Matches(currentLine);
                        foreach (Match m in fou2)
                        {
                            if (m.Success)
                            {
                                if (m.Value == "[-")
                                {

                                    currentLine = currentLine.Replace(m.Value, "[#");
                                    if (currentLine != new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text)
                                    {
                                        new TextRange(richTextBox.CaretPosition.GetLineStartPosition(0), richTextBox.CaretPosition.GetLineStartPosition(1) ?? richTextBox.CaretPosition.DocumentEnd).Text = currentLine;
                                    }
                                }
                                else
                                {
                                    currentLine = currentLine.Replace(m.Value, "##");
                                    if (currentLine != new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text)
                                    {
                                        new TextRange(richTextBox.CaretPosition.GetLineStartPosition(0), richTextBox.CaretPosition.GetLineStartPosition(1) ?? richTextBox.CaretPosition.DocumentEnd).Text = currentLine;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Kernel.Core.IsCMode = false;
        }

        public void NewPrompt()
        {
            TextRange tr = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            richTextBox.ScrollToEnd();
            richTextBox.CaretPosition = richTextBox.CaretPosition.DocumentEnd;
            richTextBox.AppendText("\r|>");
        }

        private void richTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var currentLine = new TextRange(richTextBox.CaretPosition.GetLineStartPosition(0), richTextBox.CaretPosition.GetLineStartPosition(1) ?? richTextBox.CaretPosition.DocumentEnd).Text;
            switch (e.Key)
            {
                case Key.Enter:
                    e.Handled = true;
                    Regex reg = new Regex(@"([#]).+");
                    MatchCollection fou = reg.Matches(currentLine);
                    foreach (Match m in fou)
                    {
                        if (m.Success)
                        {
                            string ccc = m.Value;
                            Console.CommandsAI(m.Value.Replace("\r", ""));
                            return;
                        }
                    }
                    NewPrompt();
                    break;
                case Key.Tab:
                    e.Handled = true;
                    break;
                case Key.Back:
                    if (currentLine == "|>\r\n")
                    {
                        e.Handled = true;
                    }
                    else if (currentLine.StartsWith("Welcome to PPHM Console window"))
                    {
                        e.Handled = true;
                    }
                    else if (currentLine.StartsWith("I really hope that you know what you are doing here!!"))
                    {
                        e.Handled = true;
                    }
                    else if (currentLine.StartsWith("You can write commands in here see every step or even better erase the whole database have fun ;)"))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        Regex rex = new Regex(@"\[\w+:\w+:\w+:\w+\]");
                        MatchCollection fon = rex.Matches(currentLine);
                        foreach (Match m in fon)
                        {
                            if (m.Success)
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    break;
            }
            base.OnPreviewKeyDown(e);
        }

    }
}