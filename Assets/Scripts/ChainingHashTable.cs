using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class ChainingHashTable<TKey, TValue> : IDictionary<TKey, TValue> 
{
    public int Capacity=16;

    protected ChainingHashTable<TKey, TValue> Chaining;

    public ChainingHashTable()
    {
        Chaining = null;
    }



    public int GetHash(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int hash = key.GetHashCode();
        return (hash & 0x7fffffff) % Capacity;
    }

    public int GetSecondaryHash(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int hash = key.GetHashCode();
        return 1 + ((hash & 0x7fffffff) % (Capacity - 1));
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
                throw new KeyNotFoundException($"키{key} 없음");
            }

        }
        set => Chaining = AddorUpDate(Chaining, key, value);
    }

    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();

    public void Add(TKey key, TValue value)
    {
        Chaining = Add(Chaining, key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }
    protected virtual ChainingHashTable<TKey, TValue> Add(ChainingHashTable<TKey, TValue> Chaining, TKey key, TValue value)
    {

        if (Chaining == null)
        {
            
        }
        else
        {
            throw new ArgumentException($"키가 존재합니다.");
        }

        return Chaining;
    }

    protected virtual ChainingHashTable<TKey, TValue> AddorUpDate(ChainingHashTable<TKey, TValue> node, TKey key, TValue value)
    {

        if (node == null)
        {
     
        }

   
        return Chaining;
    }


    public void Clear()
    {
        Chaining=null;
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
        foreach (var item in this)
        {
            array[arrayIndex++] = item;
        }
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
