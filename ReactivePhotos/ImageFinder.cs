using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using PhotoAlbum.Logging;

namespace PhotoAlbum
{
    public class ImageFinder : IImageFinder
    {
        private static string _str = "Health, Wealth and Happiness";
        private int hash = _str.GetHashCode();

        public IObservable<SearchResultViewModel> GetImages(string searchText)
        {
            return Observable.Create<SearchResultViewModel>(async observer =>
            {
                var c = new HttpClient();
                var address = default(Uri);
                var searchResults = default(string);

                var info = new DirectoryInfo(searchText);

                //BMP, GIF, EXIF, JPG, PNG, and TIFF

                IEnumerable<FileInfo> fileList1 = info.GetFiles("*.bmp", 
                    SearchOption.AllDirectories);
                IEnumerable<FileInfo> fileList2 = info.GetFiles("*.gif",
                    SearchOption.AllDirectories);
                IEnumerable<FileInfo> fileList3 = info.GetFiles("*.exif",
                    SearchOption.AllDirectories);
                IEnumerable<FileInfo> fileList4 = info.GetFiles("*.jpg",
                    SearchOption.AllDirectories);
                IEnumerable<FileInfo> fileList5 = info.GetFiles("*.png",
                    SearchOption.AllDirectories);
                IEnumerable<FileInfo> fileList6 = info.GetFiles("*.tiff",
                    SearchOption.AllDirectories);

                IEnumerable<FileInfo> _fileQuery1 =
                    from file in fileList1
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery1 = _fileQuery1.ToArray();

                IEnumerable<FileInfo> _fileQuery2 =
                    from file in fileList2
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery2 = _fileQuery2.ToArray();

                IEnumerable<FileInfo> _fileQuery3 =
                    from file in fileList3
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery3 = _fileQuery3.ToArray();

                IEnumerable<FileInfo> _fileQuery4 =
                    from file in fileList4
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery4 = _fileQuery4.ToArray();

                IEnumerable<FileInfo> _fileQuery5 =
                    from file in fileList5
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery5 = _fileQuery5.ToArray();

                IEnumerable<FileInfo> _fileQuery6 =
                    from file in fileList6
                    where file.Length > 0
                    orderby file.Name
                    select file;

                var fileQuery6 = _fileQuery6.ToArray();

                var len1 = Math.Min(25, fileQuery1.Length / 10);
                var len2 = Math.Min(25, fileQuery2.Length / 10);
                var len3 = Math.Min(25, fileQuery3.Length / 10);
                var len4 = Math.Min(25, fileQuery4.Length / 10);
                var len5 = Math.Min(25, fileQuery5.Length / 10);
                var len6 = Math.Min(25, fileQuery6.Length / 10);

                var bag1 = Add(new FileInfo[len1], new Random(), fileQuery1);
                var bag2 = Add(new FileInfo[len2], new Random(), fileQuery2);
                var bag3 = Add(new FileInfo[len3], new Random(), fileQuery3);
                var bag4 = Add(new FileInfo[len4], new Random(), fileQuery4);
                var bag5 = Add(new FileInfo[len5], new Random(), fileQuery5);
                var bag6 = Add(new FileInfo[len6], new Random(), fileQuery6);

                var finalCollection = new ConcurrentBag<FileInfo>(bag1.Union(bag2).Union(bag3).Union(bag4).Union(bag5).Union(bag6));
                Obs(finalCollection, observer);
                observer.OnCompleted();
            });
        }

        private static void Obs(ConcurrentBag<FileInfo> bag1, IObserver<SearchResultViewModel> observer)
        {
            var photos = bag1
                .Select(p => new
                {
                    Url = p.FullName,
                    Title = p.Name
                });

            foreach (var photo in photos)
            {
                var imageData = resizeImage(photo.Url, 500, 500);
                observer.OnNext(new SearchResultViewModel(imageData, photo.Title));
            }
        }

        private static ConcurrentBag<FileInfo> Add(FileInfo[] sel, Random random, FileInfo[] query)
        {
            var bag = new ConcurrentBag<FileInfo>();

            for (var i = 0; i < sel.Length; i++)
            {
                var indexToGetImageFrom = random.Next(query.Length);
                try
                {
                    bag.Add(query[indexToGetImageFrom]);
                    typeof(Log).Info("Added file : " + query[indexToGetImageFrom].FullName);
                }
                catch (Exception ex)
                {
                }
            }

            return bag;
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