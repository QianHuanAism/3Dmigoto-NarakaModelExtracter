using NMC.Helpers;
using System.IO;
using System.Windows;

namespace NMC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (Log.LogList.Count <= 0)
            {
                return;
            }

            string currentDir = Directory.GetCurrentDirectory();
            string logDir = Path.Combine(currentDir, "Logs");
            string logWritePath = Path.Combine(logDir, $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}-LOG.log");

            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            File.WriteAllLines(logWritePath, Log.LogList);
        }
    }

}
