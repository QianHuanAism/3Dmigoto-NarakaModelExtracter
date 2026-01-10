using NMC.Utils;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace NMC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            Log.Info(">>> 程序开始运行 <<<");
            this.Closed += (sender, e) =>
            {
                string currentDir = Directory.GetCurrentDirectory();
                string logDir = Path.Combine(currentDir, "Logs");
                string logWritePath = Path.Combine(logDir, $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}-LOG.log");

                if(!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                File.WriteAllLines(logWritePath, Log.LogList);
            };
        }
    }
}