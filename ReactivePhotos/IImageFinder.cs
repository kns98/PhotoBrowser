using System;

namespace PhotoAlbum
{
    public interface IImageFinder
    {
        IObservable<SearchResultViewModel> GetImages(string searchText);
    }
}
