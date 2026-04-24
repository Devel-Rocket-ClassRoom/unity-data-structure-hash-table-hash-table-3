using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HashNodeView : MonoBehaviour
{
    [SerializeField] private HashNode nodePrefab;
    [SerializeField] private ScrollRect scrollRect;
    private List<HashNode> nodeList = new List<HashNode>();

    private void Awake()
    {
        InitList();
    }

    public void InitList()
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            Destroy(nodeList[i]);
        }
        nodeList.Clear();
        for (int i = 0; i < 16; i++)
        {
            var node = Instantiate(nodePrefab, scrollRect.content);
            nodeList.Add(node);
            node.SetText($"  I: {i + 1}");
        }
    }

    public void UpdateNodeList<TKey, TValue>(IDictionary<TKey, TValue> hashTable)
    {
        if (hashTable.Count > nodeList.Count)
        {
            for (int i = nodeList.Count; i < hashTable.Count; i++)
            {
                var node = Instantiate(nodePrefab, scrollRect.content);
                nodeList.Add(node);
                node.SetText($"  I: {i + 1}");
            }
        }
        for (int i = 0; i < hashTable.Count; i++)
        {
            nodeList[i].Remove(i + 1);

            // nodeList[i].GetItem(hashTable[i].key, hashTable[i].value)
        }
    }

    public void UpdateNodeListSimple(SimpleHashTable<string, string> hashTable)
    {
        var simpleHash = hashTable.GetData();

        if (simpleHash.Length > nodeList.Count)
        {
            for (int i = nodeList.Count; i < simpleHash.Length; i++)
            {
                var node = Instantiate(nodePrefab, scrollRect.content);
                nodeList.Add(node);
                node.SetText($"  I: {i + 1}");
            }
        }
        for (int i = 0; i < simpleHash.Length; i++)
        {
            if (nodeList[i].isInItem && !simpleHash[i].IsOccupied)
            {
                nodeList[i].Remove(i + 1);
            }
            else if (simpleHash[i].IsOccupied)
            {
                nodeList[i].GetItem(simpleHash[i].Key, simpleHash[i].Value);
            }
        }
    }
}
