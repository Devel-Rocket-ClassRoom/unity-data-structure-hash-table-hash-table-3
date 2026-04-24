using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Collision
{
    Simple,
    Chaining,
    OpenAdressing,
}


public class HashTableUIManager : MonoBehaviour
{
    IDictionary<string, string> hashTable;

    public Collision collision;
    public ProbingStrategy ps;
    private string selectKey;
    [SerializeField] private HashNodeView hashNodeView;
    private string stringnum;
    void OnEnable() => HashNode.OnNodeClicked += OnSelectNode;
    void OnDisable() => HashNode.OnNodeClicked -= OnSelectNode;
    public TMP_InputField keyInput; 
    public TMP_InputField valueInput;
    public TMP_Dropdown dropdown;
    public TMP_Dropdown probDropdown;
    public Button addButton;
    public Button removeButton;
    public Button clearButton;

    public LogViewScripts logView;

    private void Awake()
    {
        hashTable = new SimpleHashTable<string, string>();   
        addButton.onClick.AddListener(OnAddClick);
        removeButton.onClick.AddListener(OnRemoveClick);
        clearButton.onClick.AddListener(OnClearClick);
        dropdown.onValueChanged.AddListener(OnHashModeChanged);
    }

    public void Start()
    {
        Debug.Log("해시테이블 시작");
    }
    private void OnAddClick()
    {
        switch (collision)
        {
            case Collision.Simple:
                Simple((SimpleHashTable<string,string>)hashTable);
                break;
            case Collision.Chaining:
                Chaining((ChainingHashTable<string, string>)hashTable);
                break;
            case Collision.OpenAdressing:
                OpenAdressing((OpenAddressingHashTable<string,string>)hashTable);
                break;

        }
        

    }

    private void OnRemoveClick()
    {
        switch (collision)
        {
            case Collision.Simple:
                if (selectKey != string.Empty) 
                    hashTable.Remove(selectKey);
                hashNodeView.UpdateNodeListSimple((SimpleHashTable<string, string>)hashTable);
                break;
            case Collision.Chaining:
                //Chaining();
                try
                {
                    hashTable.Remove(keyInput.text);
                }
                catch (ArgumentException e)
                {
                    Debug.Log("키 없음");
                }
                hashNodeView.UpdateNodeListChaining((ChainingHashTable<string, string>)hashTable);
                keyInput.text = string.Empty;
                break;
            case Collision.OpenAdressing:
                if (selectKey != string.Empty) 
                    hashTable.Remove(selectKey);
                hashNodeView.UpdateNodeListOpenAdressing((OpenAddressingHashTable<string, string>)hashTable);
                break;

        }

        //if (selectKey != string.Empty)
        //{
        //    hashTable.Remove(selectKey);
        //}
        selectKey = string.Empty;
    }
    private void OnClearClick()
    {
        hashTable.Clear();

        switch (collision)
        {
            case Collision.Simple:
                hashNodeView.UpdateNodeListSimple((SimpleHashTable<string, string>)hashTable);
                break;
            case Collision.Chaining:
                hashNodeView.UpdateNodeListChaining((ChainingHashTable<string, string>)hashTable);
                break;
            case Collision.OpenAdressing:
                hashNodeView.UpdateNodeListOpenAdressing((OpenAddressingHashTable<string, string>)hashTable);
                break;

        }

        logView.ClearLog();
    }
    public void OnSelectNode(string key)
    {
        selectKey = key;
    }
    public void OnHashModeChanged(int index)
    {
        if (Enum.TryParse(dropdown.options[index].text, out collision))
        {
            hashNodeView.InitList();
            switch (collision)
            {
                case Collision.Simple:
                    hashTable = new SimpleHashTable<string,string>();
                    break;
                case Collision.Chaining:
                    hashTable = new ChainingHashTable<string,string>();
                    break;
                case Collision.OpenAdressing:
                    hashTable = new OpenAddressingHashTable<string,string>(ps);
                    break;

            }
        }
        logView.ClearLog();
        

    }

    public void Simple(SimpleHashTable<string,string> hash)
    {
        try
        {
            hash.Add(keyInput.text, valueInput.text);
            Debug.Log($"ADD:{keyInput.text}->{valueInput.text} ");
            keyInput.text = string.Empty;
            valueInput.text = string.Empty;
            hashNodeView.UpdateNodeListSimple(hash);
        }
        catch (ArgumentException e)
        {

            Debug.Log("ADD 실패 : 키 중복");
        }
    }
    public void Chaining(ChainingHashTable<string,string> hash)
    {
        try
        {
            hash.Add(keyInput.text, valueInput.text);
            Debug.Log($"ADD:{keyInput.text}->{valueInput.text} ");
            keyInput.text = string.Empty;
            valueInput.text = string.Empty;
            hashNodeView.UpdateNodeListChaining(hash);
        }
        catch (ArgumentException e)
        {

            Debug.Log("ADD 실패 : 키 중복");
        }
    }
    public void OpenAdressing(OpenAddressingHashTable<string,string> hash)
    {
        hash.probingStrategy = ps;
        try
        {
            hash.Add(keyInput.text, valueInput.text);
            Debug.Log($"ADD:{keyInput.text}->{valueInput.text} ");
            keyInput.text = string.Empty;
            valueInput.text = string.Empty;
            hashNodeView.UpdateNodeListOpenAdressing(hash);
        }
        catch(ArgumentException e)
        {
            Debug.Log("ADD 실패 : 키 중복");
        }
            

    }
    public void selectProbing()
    {
        ps = (ProbingStrategy)Enum.Parse(typeof(ProbingStrategy), probDropdown.options[probDropdown.value].text);
        
    }
   
}
    
