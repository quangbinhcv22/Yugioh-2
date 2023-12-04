using Networks;
using QBPlugins.ScreenFlow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Network = Networks.Network;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Popup_Disconnect : Screen
{
    [SerializeField] private TMP_Text content;
    [SerializeField] private Button retryButton;


    private void Awake()
    {
        retryButton.onClick.AddListener(GotoLogin);
    }

    private void OnEnable()
    {
        var reason = Network.Cached.disconnectReason;
        content.SetText(reason);
    }

    public static void GotoLogin()
    {
        NetworkConnection.Connect();
        
        ScreenManager.ReleaseAll_ToMain();
        ScreenManager.Open(ScreenKey.Login);
    }
}