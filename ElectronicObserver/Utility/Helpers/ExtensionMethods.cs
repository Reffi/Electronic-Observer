using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
    }
}
