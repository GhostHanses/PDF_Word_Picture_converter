using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Svg;
using Image = System.Drawing.Image;


namespace Controller
{
    public partial class Picture : Page
    {
        public static DrawPic ChoosedDrawPic;
        public bool che;
        public Picture()
        {
            InitializeComponent();
            che = false;
            NameOnly.IsChecked = true;
        }

        private void enter(object sender, RoutedEventArgs e)
        {
            pop.IsOpen = true;
        }
        private void leave(object sender, RoutedEventArgs e)
        {
            pop.IsOpen = false;
        }
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Main.mainWindow.F1.Content = Main.main;
        }
        private void Up(object sender, RoutedEventArgs e)
        {
            for(int i = 0;i < Panel.Children.Count;i++)
            {
                if (((DrawPic)Panel.Children[i]).Choosed && i - 1 >= 0)
                {
                    DrawPic dp = (DrawPic)Panel.Children[i];
                    Panel.Children.Remove(Panel.Children[i]);
                    Panel.Children.Insert(i - 1,dp);
                    break;
                }
            }
            
        }
        private void Down(object sender, RoutedEventArgs e)
        {
            for(int i = 0;i < Panel.Children.Count - 1;i++)
            {
                if (((DrawPic)Panel.Children[i]).Choosed && i + 1 <= Panel.Children.Count - 1)
                {
                    //MessageBox.Show("现在的i值为"+i);
                    DrawPic dp = (DrawPic)Panel.Children[i];
                    Panel.Children.Remove(Panel.Children[i]);
                    //MessageBox.Show("现在的i值为"+i);
                    Panel.Children.Insert(i + 1,dp);
                    break;
                }
            }
            
        }
        
        private void SelectIn(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = ".jpg;.png|*.jpg;*.png";
            dialog.Multiselect = true;
            if (dialog.ShowDialog(Main.mainWindow) == false) return;
            String[] fileName = dialog.FileNames;
            
            foreach (String x in fileName)
            {
                bool ched = false;
                foreach (DrawPic dp in Panel.Children)
                {
                    if (dp != null && dp.path == x)
                    {
                        ched = true;
                        break;
                    }
                }
                if (che || !ched)
                {
                    Panel.Children.Add(new DrawPic(x));
                }
            }
        }

        private void SelectOut(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveDg = new System.Windows.Forms.SaveFileDialog();
            saveDg.Filter = ".pdf|*.pdf";
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

        private void Delete(object sender, RoutedEventArgs e)
        {
            Panel.Children.Remove(ChoosedDrawPic);
            ChoosedDrawPic = null;
            Main.Picture.up.IsEnabled = false;
            Main.Picture.down.IsEnabled = false;
            Main.Picture.delete.IsEnabled = false;
        }

        public void check(object sender, RoutedEventArgs e)
        {
            che = true;
        }
        public void uncheck(object sender, RoutedEventArgs e)
        {
            che = false;
        }

        private String t = "";
        private int limit = 0;
        private int per;
        private String[] str = null;
        
        
        private void Begin(object sender, RoutedEventArgs e)
        {
            if (Panel.Children[0] == null)
            {
                MessageBox.Show("尚未加入图片！");
            }
            else
            {
                if (OutputBox.Text.Length < 3 || !Directory.Exists((OutputBox.Text).Substring(0, 3)) ||
                    Directory.Exists(OutputBox.Text))
                {
                    MessageBox.Show("无效的输出位置！请重新输入");
                }
                else
                {
                    str = new string[Panel.Children.Count];
                    for (int i = 0;i < str.Length;i++)
                    {
                        str[i] = ((DrawPic)Panel.Children[i]).path;
                    }
                    add.IsEnabled = false;
                    br2.IsEnabled = false;
                    begin.IsEnabled = false;
                    back.IsEnabled = false;
                    Panel.IsEnabled = false;
                    OutputBox.IsEnabled = false;
                    cbx.IsEnabled = false;
                    NameOnly.IsEnabled = false;
                    FullPath.IsEnabled = false;
                    limit = 0;
                    Bar.Value = 0;
                    Bar.Visibility = Visibility.Visible;
                    per = (100-10) / Panel.Children.Count;
                    t = OutputBox.Text;
                    new Thread(Thr).Start();
                    new Thread(prog).Start();
                }
            }
        }

        public void Thr()
        {
            PdfDocument pdf = new PdfDocument();
            bool x = false;
            foreach (String arg in str)
            {
                Image image = null;
                image = Image.FromFile(arg);
                PdfPageBase page = pdf.Pages.Add(new SizeF((float)(image.Width*0.751),(float)(image.Height*0.751)),new PdfMargins(0,0));
                Bitmap scaledImage = new Bitmap(image);
                //加载缩放后的图片到PdfImage对象
                PdfImage pdfImage = PdfImage.FromImage(scaledImage);
                //在指定位置绘入图片
                page.Canvas.DrawImage(pdfImage,0,0);
                limit += per;
            }

            try
            {
                pdf.SaveToFile(t,FileFormat.PDF);
            }
            catch (Exception exception)
            {
                if (exception.ToString().Contains("正由另一进程使用"))
                {
                    x = true;
                    MessageBox.Show("文件\""+t+"\"已在另一进程中打开，请关闭后再试");
                }
            }
            limit = 100;
            if (!x)
            {
                MessageBox.Show("转换完成");
            }
            Dispatcher.BeginInvoke(new Action(delegate
            {
                pdf.Close();
                add.IsEnabled = true;
                br2.IsEnabled = true;
                begin.IsEnabled = true;
                back.IsEnabled = true;
                Panel.IsEnabled = true;
                OutputBox.IsEnabled = true;
                cbx.IsEnabled = true;
                Bar.Visibility = Visibility.Hidden;
                NameOnly.IsEnabled = true;
                FullPath.IsEnabled = true;
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
                    Thread.Sleep(5);
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        Bar.Value+=5;
                        x = Bar.Value;
                    }));
                }
                
                if (x >= 100)
                {
                    break;
                }
            }
            Thread.CurrentThread.Abort();
        }

        private void FullPath_Checked(object sender, RoutedEventArgs e)
        {
            DrawPic.fullpath = true;
            foreach (DrawPic dp in Panel.Children)
            {
                dp.Text = dp.path;
            }
        }
        private void NameOnly_Checked(object sender, RoutedEventArgs e)
        {
            DrawPic.fullpath = false;
            foreach (DrawPic dp in Panel.Children)
            {
                dp.Text = dp.path.Substring(dp.path.LastIndexOf("\\") + 1,dp.path.Length-1-dp.path.LastIndexOf("\\"));
            }
        }
    }
}