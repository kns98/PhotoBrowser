using System.Drawing;
using ReactiveUI;

namespace PhotoAlbum
{
    public class SearchResultViewModel : ReactiveObject
    {
        public SearchResultViewModel(Bitmap image, string title)
        {
            this.Image = image;
            this.Title = title;
        }

        public Bitmap Image { get; }

        public string Title { get; }
    }
}