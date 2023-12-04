using UnityEngine;

namespace QBPlugins.ScreenFlow
{
    [DefaultExecutionOrder(int.MinValue + 1)]
    public class ScreenLauncher : MonoBehaviour
    {
        private void OnEnable()
        {
            ScreenManager.Open(ScreenKey.Login);
        }
    }
}