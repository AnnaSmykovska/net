using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class ArrayExtensions
    {
        public static int CountOccurrences<T>(this T[] array, T value) where T : IEquatable<T>
        {
            return array.Count(item => item.Equals(value));
        }

        public static T[] GetUniqueElements<T>(this T[] array)
        {
            return array.Distinct().ToArray();
        }
    }
}