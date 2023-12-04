using Cysharp.Threading.Tasks;
using QBPlugins.ScreenFlow;

public class Screen_Loading : Screen
{
    private async void OnEnable()
    {
        await UniTask.Delay(500);
        ScreenManager.Open(ScreenKey.MainHome);
    }
}