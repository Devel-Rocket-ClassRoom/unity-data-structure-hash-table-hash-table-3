using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.Port;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public enum ProbingStrategy
{
    Linear,
    Quadratic,
    DoubleHash
}
public class OpenAddressingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{

    private bool[] occupied;
    private bool[] deleted;

    private TKey[] keys;
    private TValue[] values;

    private int _count = 0;


    public ProbingStrategy probingStrategy = ProbingStrategy.Linear;

    public int Capacity { get; set; }

    public float LoadFactor => (float)Count / Capacity;


    public TValue this[TKey key] 
    {
        get => TryGetValue(key, out var v) ? v : throw new KeyNotFoundException();
        set => Add(key, value);
    }

    public ICollection<TKey> Keys => keys.Where((k, i) => occupied[i] && !deleted[i]).ToList();
    public ICollection<TValue> Values => values.Where((v, i) => occupied[i] && !deleted[i]).ToList();

    public int Count => _count;

    public bool IsReadOnly => false;


    public OpenAddressingHashTable(ProbingStrategy probing)
    {
        Capacity = 16;

        occupied = new bool[Capacity];
        deleted = new bool[Capacity];
        keys = new TKey[Capacity];
        values = new TValue[Capacity];

        probingStrategy = probing;

    }

    public int GetHash(TKey key)
    {
        int hash = key.GetHashCode() & 0x7fffffff;

        for (int i = 0; i < Capacity; i++)
        {
            int index = GetProbeIndex(hash, i);

            // 빈 칸이거나 삭제된 칸, 중복 칸이면 반환
            if (!occupied[index] || deleted[index] || keys[index].Equals(key))
                return index;
        }

        // 가득차면 -1 반환
        return -1;
    }

    private int GetValidIndex(TKey key)
    {
        int hash = key.GetHashCode() & 0x7fffffff;

        for (int i = 0; i < Capacity; i++)
        {
            int index = GetProbeIndex(hash, i);

            // **빈칸일 때 즉시 반환 : 효율성을 위해서**
            if (!occupied[index] && !deleted[index]) 
                return -1; 

            // 삭제된 칸 건너뛰고 진행
            if (occupied[index] && !deleted[index] && keys[index].Equals(key))
                return index;
        }

        // 못 찾으면 반환
        return -1;
    }


    public (bool IsOccupied, TKey Key, TValue Value)[] GetData()
    {
        var result = new (bool, TKey, TValue)[Capacity];
        for (int i = 0; i < Capacity; i++)
        {
            result[i] = (occupied[i], keys[i], values[i]);
        }
        return result;
    }



    private void ReSize()
    {
        int oldCapacity = Capacity;
        bool[] oldOccupied = occupied;
        bool[] oldDeleted = deleted;
        TKey[] oldKeys = keys;
        TValue[] oldValues = values;

        Capacity *= 2;
        _count = 0;

        occupied = new bool[Capacity];
        deleted = new bool[Capacity];
        keys = new TKey[Capacity];
        values = new TValue[Capacity];

        // 이미 검증된 데이터 들이니까 개별 로직으로 싹 삽입
        // ** Add 쓰면 중복검사를 또 실행해서 비효율적임 **
        for (int i = 0; i < oldCapacity; i++)
        {
            if (oldOccupied[i])
            {
                // Add를 쓰지 말고, 순수하게 인덱스만 계산해서 넣기
                int hash = oldKeys[i].GetHashCode() & 0x7fffffff;
                int attempt = 0;
                while (true)
                {
                    int index = GetProbeIndex(hash, attempt++);
                    if (!occupied[index]) // 새 배열이니 deleted 체크 불필요
                    {
                        keys[index] = oldKeys[i];
                        values[index] = oldValues[i];
                        occupied[index] = true;
                        _count++;
                        break;
                    }
                }
            }

        }

        Debug.Log($"리사이즈 : {oldCapacity} -> {Capacity}");

    }


    public int GetProbeIndex(int hash, int attempt)
    {
        switch (probingStrategy)
        {
            case ProbingStrategy.Linear:
                return (hash + attempt) % Capacity;

            case ProbingStrategy.Quadratic:
                return (hash + attempt * attempt) % Capacity;

            case ProbingStrategy.DoubleHash:
                int step = GetSecondaryHash(hash);
                return (hash + attempt * step) % Capacity;
            default:
                throw new NotImplementedException();
        }
    }

    public int GetSecondaryHash(int hash)
    {
        // 보폭 0 방지
        return 1 + ( hash % (Capacity - 1));
    }

    // 생성 누를 때 
    public void Add(TKey key, TValue value)
    {
        if (LoadFactor > 0.6f)
        {
            ReSize();
        }

        int index = GetHash(key);

        if(index < 0)
        {
            throw new InvalidOperationException("해쉬 테이블이 가득 찼습니다.");
        }

        // 중복 
        if (occupied[index] && !deleted[index] && keys[index].Equals(key))
        {
            throw new ArgumentException($"동일한 키가 이미 존재합니다: {key}");
        }

        // 빈칸 or 삭제 칸일 때
        if (!occupied[index] || deleted[index]) 
            _count++;

        keys[index] = key;
        values[index] = value;
        occupied[index] = true;

        deleted[index] = false;

        Debug.Log($"Add : {key} -> {index}");
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

        _count = 0;
    }


    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ContainsKey(item.Key);
    }

    public bool ContainsKey(TKey key)
    {
        return GetValidIndex(key) != -1;
    }


    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < Capacity; i++)
        {
            if (occupied[i] && !deleted[i])
            {
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }
    }


    // 제거
    public bool Remove(TKey key)
    {
        int index = GetValidIndex(key);
        if (index == -1) return false;

        occupied[index] = false;
        deleted[index] = true;

        // ** 삭제된 칸은 빈칸이 아니므로, 키와 값은 초기화해주는 것이 좋음 (디버깅 편의성 위해) **
        // 추후 현업에서 대량의 데이터를 다룰 때 메모리 누수를 막아야 하기 때문에 필수. GC가 수거할 때 같이 날라가도록 함
        // 미리미리 습관 들여놓는 것이 좋음
        keys[index] = default(TKey);
        values[index] = default(TValue);

        _count--;
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }


    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = GetValidIndex(key);

        if (index != -1)
        {
            value = values[index];
            return true;
        }

        value = default;
        return false;
    }
}
