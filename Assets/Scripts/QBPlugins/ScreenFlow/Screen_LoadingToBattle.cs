using System;
using Cysharp.Threading.Tasks;
using QBPlugins.ScreenFlow;
using TMPro;
using UnityEngine;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Screen_LoadingToBattle : Screen
{
    [Space] [SerializeField] private TMP_Text txtSelfName;
    [SerializeField] private TMP_Text txtOpponentName;

    private async void OnEnable()
    {
        await UniTask.Delay(1500);
        ScreenManager.Open(ScreenKey.Battle);
    }
}