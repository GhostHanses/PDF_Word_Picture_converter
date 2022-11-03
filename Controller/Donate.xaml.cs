using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Controller
{
    public partial class Donate : Page
    {
        public Donate()
        {
            InitializeComponent();
            Assembly assembly = GetType().Assembly;
            //这时候的路径是：命名空间+文件路径，以点相隔
            //这个BitmapImage是wpf中的类
            BitmapImage bitmap1 = new BitmapImage();
            BitmapImage bitmap2 = new BitmapImage();
            bitmap1.BeginInit();//开始初始化
            System.IO.Stream streamSmall = assembly.GetManifestResourceStream("Controller.img.wechatQRcode.png");
            bitmap1.StreamSource = streamSmall;
            bitmap1.EndInit();//结束初始化
            bitmap2.BeginInit();//开始初始化
            streamSmall = assembly.GetManifestResourceStream("Controller.img.alipayQR.png");
            bitmap2.StreamSource = streamSmall;
            bitmap2.EndInit();//结束初始化
            Image1.Source = bitmap1;
            Image2.Source = bitmap2;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Main.mainWindow.F1.Content = Main.main;
        }
    }
}