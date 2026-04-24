using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HashTableUIManager : MonoBehaviour
{
    public TMP_InputField keyInput;
    public TMP_InputField valueInput;

    public Button addButton;
    public Button removeButton;
    public Button clearButton;

    public LogViewScripts logView;

    private void Awake()
    {
        addButton.onClick.AddListener(OnAddClick);
        removeButton.onClick.AddListener(OnRemoveClick);
        clearButton.onClick.AddListener(OnClearClick);
    }

    public void Start()
    {
        Debug.Log("해시테이블 시작");
    }
    private void OnAddClick()
    {
        Debug.Log("키 밸류 ADD");
    }

    private void OnRemoveClick()
    {
        Debug.Log("키 밸류 REMOVE");
    }
    private void OnClearClick()
    {
        Debug.Log("Clear : 모든 항목 삭제됨");
    }
}
