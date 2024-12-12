using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class StringExtensions
    {
        public static string Invert(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return new string(input.Reverse().ToArray());
        }

        public static int CountOccurrences(this string input, char symbol)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            return input.Count(c => c == symbol);
        }
    }
}