using QBPlugins.ScreenFlow;
using UnityEngine;
using UnityEngine.UI;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Screen_SelectMode : Screen
{
    [SerializeField] private Button btnPve;
    [SerializeField] private Button closeBtn;
    
    private void Awake()
    {
        btnPve.onClick.AddListener(ToScreen_Pve);
        closeBtn.onClick.AddListener(Close);
    }

    private void ToScreen_Pve()
    {
        ScreenManager.Open(ScreenKey.PveSelect);
        Close();
    }


    public void Close()
    {
        Destroy(gameObject);
    }
}