using QBPlugins.ScreenFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Screen_BattleResult : Screen
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button btnGoHome;


    private void Awake()
    {
        btnGoHome.onClick.AddListener(GoHome);
    }

    
    private void OnEnable()
    {
        text.SetText(Networks.Network.Cached.Fighting.isWin ? "Victory" : "Defeat");
    }
    
    private void GoHome()
    {
        ScreenManager.ReleaseAll_ToMain();
        ScreenManager.Open(ScreenKey.MainHome);
    }
}