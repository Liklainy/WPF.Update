using System.Runtime.InteropServices;
using System.Windows;

namespace WPF.Update.Common
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonCheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            // ToDo
        }

        private void ButtonRunAutoUpdater_Click(object sender, RoutedEventArgs e)
        {
            // ToDo
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label.Content = $"Version: {GetType().Assembly.GetName().Version}\n" +
                $"{RuntimeInformation.FrameworkDescription}";
        }
    }
}
