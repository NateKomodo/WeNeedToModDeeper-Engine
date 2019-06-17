using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Timers;

namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngine
    {
        public static string version = "3.2";
        public static string gameversion = GlobalStats.version;

        public static bool HasChecked = false;

        public static void CheckForUpdates()
        {
            if (HasChecked) return;
            HasChecked = true;
            try
            {
                WebClient client = new WebClient();
                ServicePointManager.ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;
                Stream stream = client.OpenRead("https://raw.githubusercontent.com/NateKomodo/WeNeedToModDeeper-Plugins/master/engine-version.txt");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    if (line.StartsWith("#")) continue;
                    string ver = line.Trim();
                    if (ver != version)
                    {
                        UnityEngine.Debug.LogError("Error: Framework out of date, Current version: " + version + ", latest is: " + ver);
                        Timer _delayTimer = new Timer
                        {
                            Interval = 5000,
                            AutoReset = false
                        };
                        _delayTimer.Elapsed += UpdateNeededCanvas;
                        _delayTimer.Start();
                        Process.Start("https://github.com/NateKomodo/WeNeedToModDeeper-installer/releases/latest");
                        HasChecked = true;
                    }
                    HasChecked = true;
                }
                HasChecked = true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Error: " + ex.Message
                    + Environment.NewLine
                    + ex.StackTrace
                    + Environment.NewLine
                    + ex.InnerException);
            }
        }

        private static void UpdateNeededCanvas(object sender, ElapsedEventArgs e)
        {
            new ModEngineCanvasOverlay("Framework is out of date, please update it. You can install the latest version using the installer (You will need to redownload it.)", 300);
        }

        public static void Main()
        {
            //Just so visual studio doesnt kill me
        }
    }
}
