using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ExtendedDictionary<T, U, V> : IEnumerable<ExtendedDictionaryElement<T, U, V>>
    {
        private List<ExtendedDictionaryElement<T, U, V>> elements;

        public ExtendedDictionary()
        {
            elements = new List<ExtendedDictionaryElement<T, U, V>>();
        }

        public void Add(T key, U value1, V value2)
        {
            elements.Add(new ExtendedDictionaryElement<T, U, V>(key, value1, value2));
        }

        public bool Remove(T key)
        {
            var element = elements.Find(e => EqualityComparer<T>.Default.Equals(e.Key, key));
            return elements.Remove(element);
        }

        public bool ContainsKey(T key)
        {
            return elements.Exists(e => EqualityComparer<T>.Default.Equals(e.Key, key));
        }

        public bool ContainsValue(U value1, V value2)
        {
            return elements.Exists(e => EqualityComparer<U>.Default.Equals(e.Value1, value1) &&
                                         EqualityComparer<V>.Default.Equals(e.Value2, value2));
        }

        public ExtendedDictionaryElement<T, U, V> this[T key]
        {
            get => elements.Find(e => EqualityComparer<T>.Default.Equals(e.Key, key));
        }

        public int Count => elements.Count;

        public IEnumerator<ExtendedDictionaryElement<T, U, V>> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}