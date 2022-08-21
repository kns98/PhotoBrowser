using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using PhotoAlbum.Logging;

namespace PhotoAlbum
{
    public class ImageFinder : IImageFinder
    {
        private readonly Random rand = new Random();

        public IObservable<SearchResultViewModel> GetImages(string searchText)
        {
            return Observable.Create<SearchResultViewModel>(async observer =>
            {
                var c = new HttpClient();
                var address = default(Uri);
                var searchResults = default(string);

                var info = new DirectoryInfo(searchText);

                IEnumerable<FileInfo> fileList = info.GetFiles("*.jpg", SearchOption.AllDirectories);

                IEnumerable<FileInfo> _fileQuery =
                    from file in fileList
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery = _fileQuery.ToArray();

                var sel = new FileInfo[50];

                var random = new Random();

                var bag = new ConcurrentBag<FileInfo>();

                for (var i = 0; i < sel.Length; i++)
                {
                    var indexToGetImageFrom = random.Next(fileQuery.Length);
                    try
                    {
                        bag.Add(fileQuery[indexToGetImageFrom]);
                        typeof(Log).Info("Added file : " + fileQuery[indexToGetImageFrom].FullName);
                    }
                    catch (Exception ex)
                    {
                    }
                }


                var photos = bag
                    .Select(p => new
                    {
                        Url = p.FullName,
                        Title = p.Name
                    });

                foreach (var photo in photos)
                {
                    //try
                    // {
                    var imageData = resizeImage(photo.Url, 500, 500);

                    observer.OnNext(new SearchResultViewModel(imageData, photo.Title));

                    // }
                    //catch (Exception ex)
                    // {
                    // Any other kind of error, we want to send to subscribers
                    //     observer.OnError(ex);
                    //}
                }

                observer.OnCompleted();
            });
        }

        public static Bitmap resizeImage(string fileName, int rectHeight, int rectWidth)
        {
            Bitmap original;
            Bitmap resizedImage;


            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                original = new Bitmap(fs);
            }


            //if the image is squared set it's height and width to the smallest of the desired
            //dimensions (our box). In the current example rectHeight < rectWidth

            if (original.Height == original.Width)
            {
                if (rectHeight >= rectWidth) rectHeight = rectWidth;

                resizedImage = new Bitmap(original, rectHeight, rectHeight);

                return resizedImage;
            }

            //calculate aspect ratio
            var aspect = original.Width / (float)original.Height;
            int newWidth, newHeight;
            //calculate new dimensions based on aspect ratio
            newWidth = (int)(rectWidth * aspect);
            newHeight = (int)(newWidth / aspect);
            //if one of the two dimensions exceed the box dimensions
            if (newWidth > rectWidth || newHeight > rectHeight)
            {
                //depending on which of the two exceeds the box dimensions set it as the box dimension and calculate the other one based on the aspect ratio
                if (newWidth > newHeight)
                {
                    newWidth = rectWidth;
                    newHeight = (int)(newWidth / aspect);
                }
                else
                {
                    newHeight = rectHeight;
                    newWidth = (int)(newHeight * aspect);
                }
            }

            resizedImage = new Bitmap(original, newWidth, newHeight);

            return resizedImage;
        }
    }
}