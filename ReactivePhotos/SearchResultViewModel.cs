using Splat;
using ReactiveUI;
using System.Drawing;

namespace PhotoAlbum
{
    public class SearchResultViewModel : ReactiveObject
    {
        private readonly Bitmap image;
        private readonly string title;

        public SearchResultViewModel(Bitmap image, string title)
        {
            this.image = image;
            this.title = title;
        }

        public Bitmap Image { get { return image; } }
        public string Title { get { return title; } }
    }
}
