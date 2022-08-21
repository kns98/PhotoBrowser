using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PhotoAlbum
{
    public class BitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = (Bitmap)value;
            return Imaging.CreateBitmapSourceFromBitmap(bitmap);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static class Imaging
        {
            public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
            {
                if (bitmap == null)
                    throw new ArgumentNullException("bitmap");

                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }
    }
}