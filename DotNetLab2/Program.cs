using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            // Демонстрація методів розширення для String
            string testString = "hello world";
            Console.WriteLine($"Original: {testString}");
            Console.WriteLine($"Inverted: {testString.Invert()}");
            Console.WriteLine($"Occurrences of 'o': {testString.CountOccurrences('o')}");

            // Демонстрація методів розширення для масивів
            int[] numbers = { 1, 2, 2, 3, 4, 4, 5 };
            Console.WriteLine($"Occurrences of 2: {numbers.CountOccurrences(2)}");
            var uniqueNumbers = numbers.GetUniqueElements();
            Console.WriteLine($"Unique elements: {string.Join(", ", uniqueNumbers)}");

            // Демонстрація роботи з ExtendedDictionary
            var dictionary = new ExtendedDictionary<string, int, string>();
            dictionary.Add("first", 1, "one");
            dictionary.Add("second", 2, "two");

            Console.WriteLine($"Contains key 'first': {dictionary.ContainsKey("first")}");
            Console.WriteLine($"Contains value (1, 'one'): {dictionary.ContainsValue(1, "one")}");

            var element = dictionary["first"];
            Console.WriteLine($"Element 'first': {element.Key}, {element.Value1}, {element.Value2}");

            Console.WriteLine($"Dictionary count: {dictionary.Count}");

            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Key}: {item.Value1}, {item.Value2}");
            }
        }
    }
}