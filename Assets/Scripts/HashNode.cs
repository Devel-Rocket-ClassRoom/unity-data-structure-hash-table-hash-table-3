using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public void GetItems(List<(string, string)> items)
    {
        var keys = items.Select(x => x.Item1).ToList();
        var values = items.Select(x => x.Item2).ToList();

        isOccupied = true;
        key = keys[0];
        value = values[0];

        SetText($"  Key: {string.Join(", ", keys)}\n  Value: {string.Join(", ", values)}");
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
