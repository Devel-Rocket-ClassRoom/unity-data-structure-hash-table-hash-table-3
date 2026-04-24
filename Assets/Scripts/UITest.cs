using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public static class RandomStringGenerator
{
    private const string chars = "abcdefghijklmnopqrstuvwxyz";

    public static string Generate(int length = 5)
    {
        char[] buffer = new char[length];
        for (int i = 0; i < length; i++)
        {
            buffer[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(buffer);
    }
}

public class UITest : MonoBehaviour
{
    SimpleHashTable<string, string> hashTable = new SimpleHashTable<string, string>();

    private string selectKey;
    [SerializeField] private HashNodeView hashNodeView;

    void OnEnable() => HashNode.OnNodeClicked += OnSelectNode;
    void OnDisable() => HashNode.OnNodeClicked -= OnSelectNode;

    public void OnButtonCreate()
    {
        hashTable.Add(RandomStringGenerator.Generate(Random.Range(5, 11)), RandomStringGenerator.Generate(Random.Range(5, 11)));
        hashNodeView.UpdateNodeListSimple(hashTable);
    }
    public void OnSelectNode(string key)
    {
        selectKey = key;
    }
    public void OnButtonDelete()
    {
        Debug.Log(selectKey);
        if (selectKey != string.Empty)
        {
            hashTable.Remove(selectKey);
            hashNodeView.UpdateNodeListSimple(hashTable);
        }
        selectKey = string.Empty;
    }
}
