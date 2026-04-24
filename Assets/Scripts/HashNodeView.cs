using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Destroy(nodeList[i].gameObject);
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
    public void UpdateNodeListOpenAdressing(OpenAddressingHashTable<string, string> hashTable)
    {
        var openAdressHash = hashTable.GetData();

        if (openAdressHash.Length > nodeList.Count)
        {
            for (int i = nodeList.Count; i < openAdressHash.Length; i++)
            {
                var node = Instantiate(nodePrefab, scrollRect.content);
                nodeList.Add(node);
                node.SetText($"  I: {i + 1}");
            }
        }
        for (int i = 0; i < openAdressHash.Length; i++)
        {
            if (nodeList[i].isInItem && !openAdressHash[i].IsOccupied)
            {
                nodeList[i].Remove(i + 1);
            }
            else if (openAdressHash[i].IsOccupied)
            {
                nodeList[i].GetItem(openAdressHash[i].Key, openAdressHash[i].Value);
            }
        }
    }
    public void UpdateNodeListChaining(ChainingHashTable<string, string> hashTable)
    {
        var chainingHash = hashTable.GetData();

        if (chainingHash.Length > nodeList.Count)
        {
            for (int i = nodeList.Count; i < chainingHash.Length; i++)
            {
                var node = Instantiate(nodePrefab, scrollRect.content);
                nodeList.Add(node);
                node.SetText($"  I: {i + 1}");
            }
        }
        for (int i = 0; i < chainingHash.Length; i++)
        {
            if (nodeList[i].isInItem && chainingHash[i].Count == 0)
            {
                nodeList[i].Remove(i + 1);
            }
            else if (chainingHash[i].Count != 0)
            {
                nodeList[i].GetItems(chainingHash[i]);
            }
        }
    }

}
