using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class SimpleHashTable<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    private struct Entry
    {
        public TKey Key;
        public TValue Value;
        public bool IsOccupied;

        public Entry(TKey key, TValue value, bool isOccupied)
        {
            Key = key;
            Value = value;
            IsOccupied = isOccupied;
        }
    }

    private Entry[] entries;

    private int capacity;
    private int count;
    public int Capacity => capacity;
    private float loadFactor = 0.75f;

    public SimpleHashTable()
    {
        capacity = 16;
        entries = new Entry[capacity];
    }

    private void Resize(int newCapacity)
    {
        if (newCapacity > capacity)
        {
            Entry[] oldEntries = entries;
            entries = new Entry[newCapacity];
            capacity = newCapacity;
            count = 0;

            foreach (var entry in oldEntries)
            {
                if (entry.IsOccupied)
                    Add(entry.Key, entry.Value);
            }
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"키 {key} 찾을 수 없음");
            }
        }
        set => AddOrUpdate(key, value);
    }

    public ICollection<TKey> Keys => entries.Where(e => e.IsOccupied).Select(e => e.Key).ToList();

    public ICollection<TValue> Values => entries.Where(e => e.IsOccupied).Select(e => e.Value).ToList();

    public int Count => count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        int hash = GetHash(key);
        if (entries[hash].IsOccupied)
        {
            throw new ArgumentException($"키 {key} 중복");
        }

        entries[hash] = new Entry(key, value, true);
        count++;

        float currentFactor = count / (float)capacity;

        if (currentFactor > loadFactor)
        {
            Resize(capacity * 2);
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    protected virtual void AddOrUpdate(TKey key, TValue value)
    {
        int hash = GetHash(key);
        if (!entries[hash].IsOccupied)
        {
            count++;
        }
        entries[hash] = new Entry(key, value, true);
    }

    public void Clear()
    {
        entries = new Entry[capacity];
        count = 0;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return TryGetValue(item.Key, out _);
    }

    public bool ContainsKey(TKey key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (Entry entry in entries)
        {
            array[arrayIndex++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (Entry entry in entries)
        {
            if (entry.IsOccupied)
            {
                yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }
    }

    public bool Remove(TKey key)
    {
        if (entries[GetHash(key)].IsOccupied)
        {
            entries[GetHash(key)] = new Entry();
            count--;

            return true;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (entries[GetHash(key)].IsOccupied)
        {
            value = entries[GetHash(key)].Value;
            return true;
        }
        else
        {
            value = default(TValue);
            return false;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int GetHash(TKey key)
    {
        return (key.GetHashCode() & 0x7FFFFFFF) % capacity;
    }
}
