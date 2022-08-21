using System.Windows;
using PhotoAlbum.Logging;

namespace PhotoAlbum.Desktop
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LogExt.Initialize();
            var service = new ImageFinder();
            DataContext = new SearchViewModel(service);
        }
    }
}