using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Controller
{
    public partial class Main : Page
    {
        public static WTP wtp;
        public static PTW ptw;
        public static Edit edit;
        public static Main main;
        public static Picture Picture;
        public static Donate donate;
        public static MainWindow mainWindow;
        
        public Main()
        {
            InitializeComponent();
        }
        
        public void PDF_to_Word(object sender, RoutedEventArgs e)
        {
            mainWindow.F1.Content = ptw;
        }
        
        public void Word_to_PDF(object sender, RoutedEventArgs e)
        {
            mainWindow.F1.Content = wtp;
        }
        public void Edit_PDF(object sender, RoutedEventArgs e)
        {
            mainWindow.F1.Content = edit;
        }
        private void Pic(object sender, RoutedEventArgs e)
        {
            mainWindow.F1.Content = Picture;
        }
        public void Donate(object sender, RoutedEventArgs e)
        {
            mainWindow.F1.Content = donate;
        }
        
    }
}