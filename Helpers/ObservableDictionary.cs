using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace TileGame.Helpers
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, ISerializable
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableDictionary() : base() { }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            NotifyReset();
        }

        protected ObservableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var items = (List<KeyValuePair<TKey, TValue>>)info.GetValue("Items", typeof(List<KeyValuePair<TKey, TValue>>));
            foreach (var item in items)
            {
                Add(item.Key, item.Value);
            }
        }

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

        public bool TryUpdate(TKey key, TValue newValue)
        {
            if (ContainsKey(key))
            {
                var oldValue = this[key];
                this[key] = newValue;
                var newItems = new List<KeyValuePair<TKey, TValue>> { new KeyValuePair<TKey, TValue>(key, newValue) };
                var oldItems = new List<KeyValuePair<TKey, TValue>> { new KeyValuePair<TKey, TValue>(key, oldValue) };
                int index = Keys.ToList().IndexOf(key);
                if (index >= 0)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, index));
                }
                return true;
            }
            return false;
        }
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var items = new List<KeyValuePair<TKey, TValue>>(this);
            info.AddValue("Items", items);
        }

        private void NotifyReset()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
