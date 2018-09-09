using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Newtonsoft.Json;

namespace HydraX
{
    class Updater
    {
        public class ApplicationRelease
        {
            [JsonProperty("tag_name")]
            public string Version { get; set; }

            [JsonProperty("html_url")]
            public string URL { get; set; }
        }

        public static void CheckForUpdates(float currentVersion, MainWindow mainWindow)
        {
            try
            {
                float latestVersion = 0;
                string latestURL = "";
                var webRequest = WebRequest.Create(@"https://api.github.com/repos/Scobalula/HydraX/releases") as HttpWebRequest;
                webRequest.UserAgent = "HydraX";

                using (var reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {

                    List<ApplicationRelease> applicationReleases = LoadGithubReleases(reader);

                    foreach (var release in applicationReleases)
                    {
                        var match = Regex.Match(release.Version, @"([-+]?[0-9]*\.?[0-9]+)");

                        if (match.Success)
                        {
                            if (float.TryParse(match.Groups[0].Value, out float releaseVersion))
                            {
                                if (releaseVersion > latestVersion)
                                {
                                    latestVersion = releaseVersion;
                                    latestURL = release.URL;
                                }
                            }
                        }
                    }
                }
                if (latestVersion > currentVersion)
                {

                    mainWindow.Dispatcher.Invoke(
                        () =>
                        {
                            var result = MessageBox.Show("A new version of HydraX is available, do you want to download it now?", "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Information);

                            if(result == MessageBoxResult.Yes)
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start(latestURL);
                                }
                                catch
                                {
                                    return;
                                }
                            }
                        });
                }
            }
            catch
            {
                return;
            }

            return;
        }

        private static List<ApplicationRelease> LoadGithubReleases(StreamReader input)
        {
            return JsonConvert.DeserializeObject<List<ApplicationRelease>>(input.ReadToEnd());
        }
    }
}