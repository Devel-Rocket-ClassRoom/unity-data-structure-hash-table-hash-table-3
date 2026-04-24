using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogViewScripts : MonoBehaviour
{
    public TextMeshProUGUI logTextPrefab;
    public Transform content;
    public ScrollRect scrollRect;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string message, string stackTrace, LogType type)
    {
        AddLog(message);
    }

    public void AddLog(string message)
    {
        TextMeshProUGUI logItem = Instantiate(logTextPrefab, content);
        logItem.text = message;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ClearLog()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}