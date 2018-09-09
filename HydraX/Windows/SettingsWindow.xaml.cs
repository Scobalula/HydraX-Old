using System.Windows;

namespace HydraX.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            ListSound.IsChecked             = Settings.ActiveSettings.ExportOptions["sound"];
            ListMapEnts.IsChecked           = Settings.ActiveSettings.ExportOptions["map_ents"];
            ListLocalize.IsChecked          = Settings.ActiveSettings.ExportOptions["localize"];
            ListRawFile.IsChecked           = Settings.ActiveSettings.ExportOptions["rawfile"];
            ListStringTable.IsChecked       = Settings.ActiveSettings.ExportOptions["stringtable"];
            ListScriptParseTree.IsChecked   = Settings.ActiveSettings.ExportOptions["scriptparsetree"];
            ListRumble.IsChecked            = Settings.ActiveSettings.ExportOptions["rumble"];
            ListAST.IsChecked               = Settings.ActiveSettings.ExportOptions["animselectortable"];
            ListAM.IsChecked                = Settings.ActiveSettings.ExportOptions["animmappingtable"];
            ListASM.IsChecked               = Settings.ActiveSettings.ExportOptions["animstatemachine"];
            ListBT.IsChecked                = Settings.ActiveSettings.ExportOptions["behaviortree"];
            ListxCam.IsChecked              = Settings.ActiveSettings.ExportOptions["xcam"];
            ListWeaponCamo.IsChecked        = Settings.ActiveSettings.ExportOptions["weaponcamo"];

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.ActiveSettings.ExportOptions["sound"]                   = ListSound.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["map_ents"]                = ListMapEnts.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["localize"]                = ListLocalize.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["rawfile"]                 = ListRawFile.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["stringtable"]             = ListStringTable.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["scriptparsetree"]         = ListScriptParseTree.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["rumble"]                  = ListRumble.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["animselectortable"]       = ListAST.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["animmappingtable"]        = ListAM.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["animstatemachine"]        = ListASM.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["behaviortree"]            = ListBT.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["xcam"]                    = ListxCam.IsChecked == true;
            Settings.ActiveSettings.ExportOptions["weaponcamo"]              = ListWeaponCamo.IsChecked == true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
