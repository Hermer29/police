using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Levels
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> source)
        {
            return source.ElementAt(UnityEngine.Random.Range(0, source.Count()));
        }
    }
}