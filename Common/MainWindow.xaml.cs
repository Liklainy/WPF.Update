using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace WPF.Update.Common
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string UpdateUrl => "http://localhost:8000";

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label.Content = $"Version: {Assembly.GetEntryAssembly().GetName().Version}\n" +
                $"{RuntimeInformation.FrameworkDescription}";
        }
    }
}
