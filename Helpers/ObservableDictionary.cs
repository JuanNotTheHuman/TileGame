using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace TileGame.Helpers
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
        }

        public new bool Remove(TKey key)
        {
            if (TryGetValue(key, out var value))
            {
                bool removed = base.Remove(key);
                if (removed)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value)));
                }
                return removed;
            }
            return false;
        }
        public ObservableDictionary() : base() { }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            NotifyReset();
        }
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        private void NotifyReset()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}