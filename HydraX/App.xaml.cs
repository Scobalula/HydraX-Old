using System.Data;
using System.Linq;
using System.Windows;
using HydraLib;

namespace HydraX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Current.MainWindow;
            mainWindow.InitExport(mainWindow.Assets.SelectedItems.Cast<Asset>().ToList(), "No assets selected to export");
        }
    }
}
