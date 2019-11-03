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

        public static int Clamp(this int val, int min = 0, int max = int.MaxValue)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        // todo: replace with Math.Clamp after we get dotnet core
        public static T Clamp<T>(this T val, T min, T max) where T : struct, IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }
    }
}
