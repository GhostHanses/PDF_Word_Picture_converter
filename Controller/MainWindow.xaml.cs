using System;
using System.Reflection;
using System.Windows;

namespace Controller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            Main.mainWindow = (MainWindow)Application.Current.MainWindow;
            Main.main = new Main();
            Main.donate = new Donate();
            Main.wtp = new WTP();
            Main.ptw = new PTW();
            Main.edit = new Edit();
            Main.Picture = new Picture();
            Main.Picture.up.IsEnabled = false;
            Main.Picture.down.IsEnabled = false;
            Main.Picture.delete.IsEnabled = false;
            Main.Picture.cbx.IsEnabled = true;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Closing += Window_Closing;
            F1.Content = Main.main;
        }

        

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resourceName = "Controller.lib." + new AssemblyName(args.Name).Name + ".dll";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(114514);
        }
    }
}