using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HashNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mText;
    private string key;
    private string value;
    private bool isOccupied;
    private Color firstColor;
    public static event Action<string> OnNodeClicked;

    private void Awake()
    {
        firstColor = GetComponent<Image>().color;
    }
    public void SetText(string text)
    {
        mText.text = text;
    }

    public void GetItem(string key, string value)
    {
        this.key = key;
        this.value = value;
        isOccupied = true;
        SetText($"  Key: {key}\n  Value: {value}");
        this.GetComponent<Image>().color = Color.green;
    }

    public void Remove(int index)
    {
        this.key = default(string);
        this.value = default(string);
        isOccupied = false;
        SetText($"  I: {index}");
        this.GetComponent<Image>().color = firstColor;
    }

    public void OnClickNode()
    {
        if (isOccupied)
        {
            OnNodeClicked?.Invoke(key);
        }
    }

    public bool isInItem => isOccupied;
}
