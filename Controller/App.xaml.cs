using System;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace Controller
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            InitializeComponent();
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {            
            MessageBox.Show(Environment.NewLine + e.Exception.Message);
            //Shutdown(1);
            e.Handled = true;
        }
    }
}