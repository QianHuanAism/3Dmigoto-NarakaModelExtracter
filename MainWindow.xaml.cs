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
            Console.Clear();
        }
    }
}