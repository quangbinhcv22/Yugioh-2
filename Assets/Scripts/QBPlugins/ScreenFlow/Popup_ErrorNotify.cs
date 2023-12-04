using System;
using QBPlugins.ScreenFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Popup_ErrorNotify : Screen
{
    private static string Title;
    private static string Content;
    private static Action OnClick;

    public static void Open(string title, string content, Action onClick = null)
    {
        Title = title;
        Content = content;
        OnClick = onClick;

        ScreenManager.Open(ScreenKey.ErrorNotify);
    }

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text content;
    [SerializeField] private Button retryButton;


    private void Awake()
    {
        retryButton.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        title.SetText(Title);
        content.SetText(Content);
    }

    private void Close()
    {
        OnClick?.Invoke();
        Destroy(gameObject);
    }
}