using System.Windows;

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
            var service = new ImageFinder();
            DataContext = new SearchViewModel(service);
        }
    }
}