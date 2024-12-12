using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ExtendedDictionaryElement<T, U, V>
    {
        public T Key { get; set; }
        public U Value1 { get; set; }
        public V Value2 { get; set; }

        public ExtendedDictionaryElement(T key, U value1, V value2)
        {
            Key = key;
            Value1 = value1;
            Value2 = value2;
        }
    }
}