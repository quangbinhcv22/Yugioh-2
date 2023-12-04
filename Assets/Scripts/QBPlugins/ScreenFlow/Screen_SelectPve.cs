using QBPlugins.ScreenFlow;
using UnityEngine.UI;
using Screen = QBPlugins.ScreenFlow.Screen;

public class Screen_SelectPve : Screen
{
    public Button fightButton;
    public Button backButton;
    
    private void Awake()
    {
        fightButton.onClick.AddListener(StartGame);
        backButton.onClick.AddListener(Back);
    }

    private async void Back()
    {
        await ScreenManager.Close_CurrentWindow();
        ScreenManager.Open(ScreenKey.MainHome);
    }

    private void StartGame()
    {
        if (Screen_Battle.Current)
        {
            Screen_Battle.Current.gameObject.SetActive(true);
        }
        else
        {
            ScreenManager.Open(ScreenKey.Battle);

            // Instantiate(battleScreen, ScreenManager.Main.transform);
        }
        
        // gameObject.SetActive(false);
    }
}