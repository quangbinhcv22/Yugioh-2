using System;
using System.Collections;
using QBPlugins.ScreenFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Screen_MainHome : Screen
{
    [SerializeField] private Button btnSelectMode;


    [Space] [SerializeField] private TMP_Text uiDisplayName;
    [SerializeField] private TMP_Text uiGold;
    [SerializeField] private TMP_Text uiRuby;

    [Space] [SerializeField] private Image uiAvt;
    [SerializeField] private Image uiCover;

    [Space] [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private TMP_Text txtRank;

    // [SerializeField] private int uiLevel;
    // [SerializeField] private string uiAvatarImage;
    // [SerializeField] private long uiId;
    // [SerializeField] private int uiExp;
    // [SerializeField] private string uiUsername;

    private void Awake()
    {
        btnSelectMode.onClick.AddListener(ToScreen_Battle);
    }


    private void OnEnable()
    {
        var info = Network.Cached.playerInfo;

        uiDisplayName.SetText(info.displayName);

        uiGold.SetText($"{info.gold:N0}");
        uiRuby.SetText($"{info.ruby:N0}");

        txtLevel.SetText($"{info.level}");
        txtRank.SetText($"{info.rank}");

        StartCoroutine(LoadAvt(info.avatarImage));
        StartCoroutine(LoadCover(info.coverImage));
    }

    IEnumerator LoadAvt(string url)
    {
        var www = new WWW(url);
        yield return www;

        uiAvt.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }

    IEnumerator LoadCover(string url)
    {
        var www = new WWW(url);
        yield return www;

        uiCover.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }

    private void ToScreen_Battle()
    {
        ScreenManager.Open(ScreenKey.Matching);

        // ScreenManager.Open(ScreenKey.SelectMode);
    }
}