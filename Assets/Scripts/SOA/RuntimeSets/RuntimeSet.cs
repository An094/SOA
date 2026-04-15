using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    private readonly List<T> _items = new();

    private void OnEnable() => _items.Clear();

    public void Add(T item)
    {
        if(!_items.Contains(item))
        {
            _items.Add(item);
        }
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public int Count => _items.Count;
    public T this[int i] => _items[i];
    public bool Contains(T item) => _items.Contains(item);

    public IEnumerable<T> Items => new List<T>(_items);
}
