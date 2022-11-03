using System.Windows;
using System.Windows.Controls;

namespace Controller
{
    public partial class Edit : Page
    {
        public Edit()
        {
            InitializeComponent();
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Main.mainWindow.F1.Content = Main.main;
            
        }
    }
}