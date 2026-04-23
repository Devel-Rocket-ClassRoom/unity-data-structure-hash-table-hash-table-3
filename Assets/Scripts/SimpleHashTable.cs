using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHashTable<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    private TValue[] values;
    private int capacity;
    private int size;
    public int Capacity => capacity;
    public int Size => size;
    private float loadFactor = 0.75f;
    public SimpleHashTable()
    {
        capacity = 16;
        values = new TValue[capacity];
    }

    private void Resize(int newCapacity)
    {
        if (newCapacity > capacity)
        {
            TValue[] newContainer = new TValue[newCapacity];
            for (int i = 0; i < capacity; i++)
            {
                newContainer[i] = values[i];
            }
            capacity = newCapacity;
            values = newContainer;
        }
    }

    public TValue this[TKey key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();

    public void Add(TKey key, TValue value)
    {
        int hash = key.GetHashCode() % capacity;
        values[hash] = value;
        size++;

        float currentFactor = size / (float)capacity;

        if (currentFactor > loadFactor)
        {

        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public bool ContainsKey(TKey key)
    {
        throw new System.NotImplementedException();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    public bool Remove(TKey key)
    {
        throw new System.NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
