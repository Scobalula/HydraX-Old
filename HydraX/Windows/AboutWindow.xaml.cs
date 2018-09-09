using System.Diagnostics;
using System.Windows;

namespace HydraX.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.buymeacoffee.com/Scobalula");
        }

        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://philmaher.me/HydraX/");
        }
    }
}
