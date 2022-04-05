using System;
using System.Collections.Generic;
using System.Linq;

namespace ThemeMeUp.ConsoleApp
{
    public static class IEnumerableExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> collection, Random rng)
            => collection.ElementAt(rng.Next(0, collection.Count()));
    }
}
