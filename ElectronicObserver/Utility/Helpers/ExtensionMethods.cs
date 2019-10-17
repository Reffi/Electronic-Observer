using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Utility.Helpers
{ 
    // I have no better name right now
    public static class ExtensionMethods
    {
        public static VisibleFits Sum(this IEnumerable<VisibleFits> source) => 
            source.Aggregate(new VisibleFits(), (x, y) => x + y);

        public static VisibleFits Sum<T>(this IEnumerable<T> source, Func<T, VisibleFits> selector) =>
            source.Select(selector).Aggregate(new VisibleFits(), (x, y) => x + y);

        public static string Display(this Enum enumValue) =>
            enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? enumValue.ToString();

        public static BitmapImage ToImageSource(this Image image)
        {
            // ImageSource ...
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();

            MemoryStream ms = new MemoryStream();

            var a = PixelFormat.Format32bppArgb;

            // Save to a memory stream...
            image.Save(ms, ImageFormat.Png);

            // Rewind the stream...
            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...
            bi.StreamSource = ms;

            bi.EndInit();

            return bi;
        }
    }
}
