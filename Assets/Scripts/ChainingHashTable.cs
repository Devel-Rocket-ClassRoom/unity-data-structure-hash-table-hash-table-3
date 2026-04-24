<<<<<<< HEAD
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
=======
using System;
using System.Collections;
using System.Collections.Generic;
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
<<<<<<< HEAD
using static UnityEditor.Experimental.GraphView.Port;
using static UnityEngine.Rendering.DebugUI;

public class ChainingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    public List<List<KeyValuePair<TKey, TValue>>> buckets;
    private int count;
    public int Capacity = 16;

    public ChainingHashTable()
    {
        buckets = new List<List<KeyValuePair<TKey, TValue>>>();

        for (int i = 0; i < Capacity; i++)
        {
            buckets.Add(new List<KeyValuePair<TKey, TValue>>());
        }
    }


    public int GetHash(TKey key)
    {
        if (key == null)
        { 
            return -1;
        }

        int hash = key.GetHashCode();
        return (hash & 0x7fffffff) % buckets.Count;
    }

    private void LoadFactor()
    {
        float loadFactor = 0.75f;

        if ((float)count / Capacity > loadFactor)
        {
             Resize();
        }
    }

    private void Resize()
    {
        var oldBuckets= buckets;
        buckets = new List<List<KeyValuePair<TKey, TValue>>>();
        Capacity *= 2;
        for (int i = 0; i < Capacity; i++)
        {
            buckets.Add(new List<KeyValuePair<TKey, TValue>>());
        }
        foreach (var bk in oldBuckets)
        {
            foreach (var pire in bk)
            {
                var key = pire.Key;
                var Value = pire.Value;
                buckets[GetHash(key)].Add(new KeyValuePair<TKey, TValue>(key, Value));
            }
        }
    }

    public List<( TKey key, TValue Value)>[] GetData()
    {

        var Buckets = buckets;
        var data = new List<( TKey key, TValue Value)>[Capacity];

        for (int i = 0; i < Capacity; i++)
        {
            var chain = new List<( TKey key, TValue Value)>();
            foreach (var list in Buckets[i])
            {
                chain.Add((list.Key, list.Value));
            }
            data[i] = chain;
        }
        return data;

    }

    public TValue this[TKey key]
=======
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
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
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
<<<<<<< HEAD
        set => AddorUpDate(key, value);
    }



=======
        set => Chaining = AddorUpDate(Chaining, key, value);
    }

>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

<<<<<<< HEAD
    public int Count => count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        int index = GetHash(key);
        var bucket = buckets[index];
        foreach (var kv in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(kv.Key, key))
            {
                throw new ArgumentException($"이미 존재하는 키: {key}");
            }
        }
        bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
        LoadFactor();
        count++;
    }
    protected virtual void AddorUpDate(TKey key, TValue value)
    {
        int index = GetHash(key);
        var bucket = buckets[index];

        for (int i = 0; i < bucket.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(bucket[i].Key, key))
            {
                bucket[i] = new KeyValuePair<TKey, TValue>(key, value);
                return;
            }
        }
        bucket.Add(new KeyValuePair<TKey, TValue>(key, value));
        LoadFactor();
        count++;
    }

 

    public void Clear()
    {
        foreach (var bucket in buckets)
        {
            bucket.Clear();
        }

        count = 0;
=======
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
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
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
<<<<<<< HEAD
        foreach (var bucket in buckets)
        {
            foreach (var kv in bucket)
            {
                yield return kv;
            }
        }
=======
        throw new System.NotImplementedException();
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
    }

    public bool Remove(TKey key)
    {
<<<<<<< HEAD
        int index = GetHash(key);
        var bucket = buckets[index];

        for (int i = 0; i < bucket.Count; i++)
        {
            if (EqualityComparer<TKey>.Default.Equals(bucket[i].Key, key))
            {
                bucket.RemoveAt(i);
                count--;
                return true;
            }
        }

        return false;
=======
        throw new System.NotImplementedException();
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
<<<<<<< HEAD
        if (Contains(item))
        {
            return Remove(item.Key);
        }
        return false;
=======
        throw new System.NotImplementedException();
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
<<<<<<< HEAD
        int index = GetHash(key);
        var bucket = buckets[index];

        foreach (var kv in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(kv.Key, key))
            {
                value = kv.Value;
                return true;
            }
        }

        value = default;
        return false;
=======
        throw new System.NotImplementedException();
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

<<<<<<< HEAD
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
    }
=======
>>>>>>> d183982d15527664a0b909796fba8692bcde37ed
}
