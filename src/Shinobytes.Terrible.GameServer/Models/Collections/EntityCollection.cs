using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Terrible.Models
{
    public class EntityCollection<T> : IList<T>
    {
        private readonly List<T> items = new List<T>();

        public EntityCollection() { }

        public EntityCollection(IEnumerable<T> items)
        {
            this.items = new List<T>(items);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public T this[int index]
        {
            get => this.items[index];
            set => this.items[index] = value;
        }

        public EntityCollection<T> Except(System.Func<T, bool> predicate)
        {
            var item = this.items.FirstOrDefault(predicate);
            return new EntityCollection<T>(this.items.Except(new[] { item }));
        }

        public EntityCollection<T> Except(T item)
        {
            return new EntityCollection<T>(this.items.Except(new [] { item }));
        }
    }
}