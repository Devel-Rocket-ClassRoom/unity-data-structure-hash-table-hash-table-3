using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class OpenAddressingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{

    private bool[] occupied;
    private bool[] deleted;

    private TKey[] keys;
    private TValue[] values;

    public enum ProbingStrategy
    {
        Linear,
        Quadratic,
        DoubleHash
    }

    public ProbingStrategy probingStrategy = ProbingStrategy.Linear;

    public int Capacity { get; private set; }

    public float LoadFactor => (float)Count / Capacity;


    public TValue this[TKey key] 
    {   
        get => throw new System.NotImplementedException(); 
        set => throw new System.NotImplementedException(); 
    }

    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();


    public OpenAddressingHashTable(ProbingStrategy probing)
    {
        occupied = new bool[Capacity];
        deleted = new bool[Capacity];
        keys = new TKey[Capacity];
        values = new TValue[Capacity];
    }

    public int GetHash(TKey key)
    {
        int hash = key.GetHashCode();

        int index = (hash & 0x7fffffff) % Capacity;
        int attempt = 0;

        while (occupied[index])
        {

            if (!deleted[index])
            {
                attempt++;
                index = GetProbeIndex(hash, attempt);
            }

        }

        return index;
    }

    private void ReSize()
    {
        int oldCapacity = Capacity;
        bool[] oldOccupied = occupied;
        bool[] oldDeleted = deleted;
        TKey[] oldKeys = keys;
        TValue[] oldValues = values;

        Capacity *= 2;

        occupied = new bool[Capacity];
        deleted = new bool[Capacity];
        keys = new TKey[Capacity];
        values = new TValue[Capacity];

        // 새로 싹 삽입
        for (int i = 0; i < oldCapacity; i++)
        {
            if (oldOccupied[i])
            {
                Add(oldKeys[i], oldValues[i]);
            }
        }
        ;
    }


    public int GetProbeIndex(int key, int attempt)
    {
        switch (probingStrategy)
        {
            case ProbingStrategy.Linear:
                return (key + attempt) % Capacity;

            case ProbingStrategy.Quadratic:
                return (key + attempt * attempt) % Capacity;

            case ProbingStrategy.DoubleHash:
                int step = GetSecondaryHash(key);
                return (key + attempt * step) % Capacity;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public int GetSecondaryHash(int key)
    {
        return 1 + (key % (Capacity - 1));
    }

    // 생성 누를 때 
    public void Add(TKey key, TValue value)
    {
        if (LoadFactor > 0.6f)
        {
            ReSize();
        }

        occupied[GetHash(key)] = true;
        keys[GetHash(key)] = key;
        values[GetHash(key)] = value;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    // 클리어
    public void Clear()
    {
        occupied = new bool[Capacity];
        deleted = new bool[Capacity];
        keys = new TKey[Capacity];
        values = new TValue[Capacity];
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


    // 제거
    public bool Remove(TKey key)
    {
        deleted[GetHash(key)] = true;

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
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
