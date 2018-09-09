using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Win32;
using HydraX.Windows;
using HydraLib;
using PhilUtil;

namespace HydraX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, Func<Process, List<Asset>>> SupportedGames = new Dictionary<string, Func<Process, List<Asset>>>() 
        {
            { "BlackOps3" , T7Util.Load }
        };

        /// <summary>
        /// Current Loaded Assets
        /// </summary>
        List<Asset> ActiveAssets = new List<Asset>();

        public float CurrentVersion = 0.70f;

        public string CurrentVersionString = "v0.7.0-alpha";

        public MainWindow()
        {
            new System.Threading.Thread(delegate () { Updater.CheckForUpdates(CurrentVersion, this); }).Start();

            Settings.ActiveSettings = new Settings();
            // Load Settings
            try
            {
                Settings.Load("Settings.json");
            }
            catch
            {
                LoggingUtil.ActiveLogger.Log("Settings file not found, writing a new settings file.", MessageType.INFO);
                try
                {
                    Settings.Write("Settings.json");
                }
                catch(Exception exception)
                {
                    LoggingUtil.ActiveLogger.Log("Failed to Write Settings File", MessageType.ERROR);
                    LoggingUtil.ActiveLogger.Log(String.Format("Error: {0}", exception.Message), MessageType.ERROR);
                }
            }
            LoggingUtil.ActiveLogger.CloseStream();


            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

            Process[] pname;
            string errorMessage = "HydraX failed to find a supported game. Please ensure one of them is running.";

            foreach (var game in SupportedGames)
            {
                try
                {
                    pname = Process.GetProcessesByName(game.Key);

                    if (pname.Length > 0)
                    {
                        ClearLoadedAssets();

                        ActiveAssets = game.Value(pname[0]);

                        ActiveAssets.ForEach(x => Assets.Items.Add(x));

                        AssetLoadedLabel.Content = String.Format("{0} Assets Loaded", ActiveAssets.Count);

                        return;
                    }
                }
                catch
                {

                    errorMessage = String.Format("Failed to load {0}. This version of the game may not be supported", game.Key);
                    MessageBox.Show
                        (
                            errorMessage, 
                            "ERROR", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Exclamation
                        );
                    LoggingUtil.ActiveLogger.Log(errorMessage, MessageType.ERROR);
                    return;
                }
                finally
                {
                    LoggingUtil.ActiveLogger.CloseStream();
                }

            }


            MessageBox.Show
                (
                    errorMessage, 
                    "ERROR", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Exclamation
                );
        }

        private void ExportAssets(List<Asset> assets, ProgressWindow progressWindow, out int assetsExported, out int assetsFailed)
        {
            assetsExported = 0;
            assetsFailed = 0;
            float progress = 0; 

            for (int i = 0; i < assets.Count && progressWindow.UpdateProgress(progress); i++)
            {
                LoggingUtil.ActiveLogger.Log(String.Format("Exporting {0}", assets[i].Path), MessageType.INFO);
                try
                {
                    assets[i].ExportFunction(assets[i]);

                    LoggingUtil.ActiveLogger.Log(String.Format("Exported {0} successfully.", assets[i].Path), MessageType.INFO);

                    assetsExported++;
                }
                catch(Exception e)
                {
                    LoggingUtil.ActiveLogger.Log(e.ToString(), MessageType.ERROR);
                    assetsFailed++;
                }

                progress = (i / (float)(assets.Count)) * (float)100.000;
            }
        }

        /// <summary>
        /// Initiates Export with Asset List and a Message to Display is no assets are given
        /// </summary>
        /// <param name="assets">Assets</param>
        /// <param name="zeroAssetsMessage">Message to show if no assets are given</param>
        public void InitExport(List<Asset> assets, string zeroAssetsMessage)
        {
            if (assets.Count == 0)
            {
                MessageBox.Show(zeroAssetsMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                int assetsExported = 0;
                int assetsFailed = 0;
                ProgressWindow progressWindow = new ProgressWindow
                {
                    Owner = this
                };

                ExportFinishedWindow exportFinishedWindow = new ExportFinishedWindow
                {
                    Owner = this
                };

                progressWindow.label.Content = String.Format("Exporting {0} Assets....", assets.Count);
                Dispatcher.BeginInvoke(new Action(() => progressWindow.ShowDialog()));
                DimBox.Visibility = Visibility.Visible;
                new System.Threading.Thread(
                    delegate ()
                    {
                        ExportAssets(assets, progressWindow, out assetsExported, out assetsFailed);
                        progressWindow.isWorking = false;
                        Dispatcher.BeginInvoke(new Action(() => progressWindow.Close()));
                        string message = String.Format
                        (
                            "{0} Asset{1} Exported. {2} Failed to export.", 
                            assetsExported,
                            assetsExported == 1 ? "" : "s",
                            assetsFailed
                        );
                        LoggingUtil.ActiveLogger.CloseStream();
                        Dispatcher.Invoke(() =>
                        {
                            exportFinishedWindow.ExportedAssets.Content = message;
                            exportFinishedWindow.ShowDialog();
                            DimBox.Visibility = Visibility.Hidden;
                            Activate();
                        });

                    }).Start();
            }
        }

        private void  ExportSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            InitExport(Assets.SelectedItems.Cast<Asset>().ToList(), "No assets selected to export");
        }

        private void ExportAllButton_Click(object sender, RoutedEventArgs e)
        {
            InitExport(Assets.Items.Cast<Asset>().ToList(), "No assets listed to export");
        }

        private void ClearAssets_Click(object sender, RoutedEventArgs e)
        {
            ClearLoadedAssets();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSearch();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ExecuteSearch();
        }

        /// <summary>
        /// Handles executing search
        /// </summary>
        private void ExecuteSearch()
        {
            // Check if there is a query
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                // Clear current list
                Assets.Items.Clear();
                string[] query = SearchBox.Text.Split();
                ActiveAssets.ForEach(asset =>
                {
                    if (query.Any(search => asset.Path.Contains(search)))
                    {
                        Assets.Items.Add(asset);
                    }
                });
            }
            else
            {
                ClearSearch();
            }
        }

        /// <summary>
        /// Handles Clearing Search and Listed Items
        /// </summary>
        private void ClearSearch()
        {
            // Check if a search was made (these would not equal)
            if (Assets.Items.Count != ActiveAssets.Count)
            {
                Assets.Items.Clear();
                SearchBox.Clear();
                ActiveAssets.ForEach(x => Assets.Items.Add(x));
            }
        }

        public void ClearLoadedAssets()
        {
            T7Util.Clear();
            ActiveAssets.Clear();
            Assets.Items.Clear();
            AssetLoadedLabel.Content = String.Format("{0} Assets Loaded", ActiveAssets?.Count);
        }

        public void ProcessFastFile(string fileName)
        {
            ProgressWindow progressWindow = new ProgressWindow
            {
                Owner = this
            };
            progressWindow.label.Content = "Decompressing Fast File....";
            Dispatcher.BeginInvoke(new Action(() => progressWindow.ShowDialog()));
            ClearLoadedAssets();
            DimBox.Visibility = Visibility.Visible;
            new System.Threading.Thread(
                delegate ()
                {
                    T7Util.ActiveFastFile = new T7Util.FastFile();
                    T7Util.ActiveFastFile.Decode(fileName, progressWindow.UpdateProgress);
                    progressWindow.SwitchProgressMode("Searching for assets....");
                    ActiveAssets = T7Util.ActiveFastFile.Load(progressWindow.CheckUserCancel);
                    progressWindow.isWorking = false;
                    Dispatcher.BeginInvoke(new Action(() => progressWindow.Close()));
                    LoggingUtil.ActiveLogger.CloseStream();
                    Dispatcher.Invoke(
                        () =>
                        {
                            ActiveAssets.ForEach(x => Assets.Items.Add(x));
                            AssetLoadedLabel.Content = String.Format("{0} Assets Loaded", ActiveAssets.Count);
                            DimBox.Visibility = Visibility.Hidden;
                        }
                        );
                }).Start();

        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow()
            {
                Owner = this
            };
            DimBox.Visibility = Visibility.Visible;

            aboutWindow.ShowDialog();

            DimBox.Visibility = Visibility.Hidden;
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Fast File (.ff)|*.ff"
            };

            if (dlg.ShowDialog() == true)
            {
                ProcessFastFile(dlg.FileName);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClearLoadedAssets();

            try
            {
                Settings.Write("Settings.json");
            }
            catch (Exception exception)
            {
                LoggingUtil.ActiveLogger.Log("Failed to Write Settings File", MessageType.ERROR);
                LoggingUtil.ActiveLogger.Log(String.Format("Error: {0}", exception.Message), MessageType.ERROR);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow()
            {
                Owner = this
            };

            DimBox.Visibility = Visibility.Visible;

            settingsWindow.ShowDialog();

            DimBox.Visibility = Visibility.Hidden;
        }
    }
}
