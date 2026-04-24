using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HashTableUIManager : MonoBehaviour
{
    SimpleHashTable<string, string> hashTable = new SimpleHashTable<string, string>();

    private string selectKey;
    [SerializeField] private HashNodeView hashNodeView;
    private string stringnum;
    void OnEnable() => HashNode.OnNodeClicked += OnSelectNode;
    void OnDisable() => HashNode.OnNodeClicked -= OnSelectNode;
    public TMP_InputField keyInput;
    public TMP_InputField valueInput;
    public TMP_Dropdown dropdown;
    public Button addButton;
    public Button removeButton;
    public Button clearButton;

    public LogViewScripts logView;

    private void Awake()
    {
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
        
        try
        {
            hashTable.Add(keyInput.text, valueInput.text);
            Debug.Log($"ADD:{keyInput.text}->{valueInput.text} ");
            keyInput.text = string.Empty;
            valueInput.text = string.Empty;
            hashNodeView.UpdateNodeListSimple(hashTable);
            
        }
        catch(ArgumentException e)
        {
            
            Debug.Log("ADD 실패 : 키 중복");
        }

    }

    private void OnRemoveClick()
    {
        
        if (selectKey != string.Empty)
        {
            hashTable.Remove(selectKey);
            hashNodeView.UpdateNodeListSimple(hashTable);
        }
        selectKey = string.Empty;
    }
    private void OnClearClick()
    {
        hashTable.Clear();
        hashNodeView.UpdateNodeListSimple(hashTable); //simple 전용코드(통합코드필요)
        logView.ClearLog();
    }
    public void OnSelectNode(string key)
    {
        selectKey = key;
    }
    private void OnHashModeChanged(int index)
    {
        logView.ClearLog();
        hashTable.Clear();
        hashNodeView.UpdateNodeListSimple(hashTable);
    }
}
