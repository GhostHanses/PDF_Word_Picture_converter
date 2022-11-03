using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Spire.Pdf;

namespace Controller
{
    public partial class PTW : Page
    {

        public PTW()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Main.mainWindow.F1.Content = Main.main;
        }

        private void SelectIn(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = ".pdf|*.pdf";
            dialog.Multiselect = false;
            if (dialog.ShowDialog(Main.mainWindow) == false) return;
            String _fileName = dialog.FileName;
            InputBox.Text = _fileName;
        }
        private void SelectOut(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveDg = new System.Windows.Forms.SaveFileDialog();
            saveDg.Filter = ".docx|*.docx";
            saveDg.FileName = "";
            saveDg.AddExtension = true;
            saveDg.RestoreDirectory = true;
            if (saveDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事
                string filePath = saveDg.FileName;
                OutputBox.Text = filePath;
            }
        }

        private void Begin(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(InputBox.Text))
            {
                MessageBox.Show("无效的文件路径，请重新输入！");
            }
            else
            {
                if (OutputBox.Text.Length < 3 || !Directory.Exists((OutputBox.Text).Substring(0,3)) || Directory.Exists(OutputBox.Text))
                {
                    MessageBox.Show("无效的输出位置！请重新输入");
                }
                else
                {
                    br1.IsEnabled = false;
                    br2.IsEnabled = false;
                    begin.IsEnabled = false;
                    back.IsEnabled = false;
                    InputBox.IsEnabled = false;
                    OutputBox.IsEnabled = false;
                    t1 = InputBox.Text;
                    t2 = OutputBox.Text;
                    new Thread(_thread).Start();
                    new Thread(prog).Start();
                }
            }
        }

        private string t1 = "";
        private string t2 = "";
        private int limit = 0;

        private void _thread()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                Bar.Visibility = Visibility.Visible;
                Bar.Value = 0;
            }));
            limit = 0;
            PdfDocument pdf = new PdfDocument(t1);
            limit = 90;
            pdf.SaveToFile(t2, FileFormat.DOCX);
            limit = 100;
            MessageBox.Show("转换完成");
            Dispatcher.BeginInvoke(new Action(delegate
            {
                pdf.Close();
                br1.IsEnabled = true;
                br2.IsEnabled = true;
                begin.IsEnabled = true;
                back.IsEnabled = true;
                InputBox.IsEnabled = true;
                OutputBox.IsEnabled = true;
                Bar.Visibility = Visibility.Hidden;
            }));
            Thread.CurrentThread.Abort();
        }

        private void prog()
        {
            double x = 0;
            while (limit <= 100)
            {
                while (x < limit)
                {
                    Thread.Sleep(10);
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        Bar.Value+=2;
                        x = Bar.Value;
                    }));
                }
                if (x >= 100)
                {
                    break;
                }
            }
        }
    }
}