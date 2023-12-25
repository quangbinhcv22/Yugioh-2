using System;
using Cysharp.Threading.Tasks;
using gameplay.manager;
using Networks;
using UnityEngine;
using Network = Networks.Network;

namespace Testing
{
    public class ReconnectingTest : MonoBehaviour
    {
        private void Update()
        {
            var disconnect = Application.internetReachability == NetworkReachability.NotReachable;

            if (disconnect && !waitHaveConnect)
            {
                var loggedThisSession = !string.IsNullOrEmpty(Network.Cached.cachedLoginRequest.username);
                if (loggedThisSession)
                {
                    TryReconnect();
                }
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (Server_PhaseManager.main != null)
            {
                Network.Request.LoadGameState();
            }
        }

        private bool waitHaveConnect;

        private async void TryReconnect()
        {
            waitHaveConnect = true;
            Screen_Reconnecting.singleton.Open();

            await UniTask.WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);

            waitHaveConnect = false;

            NetworkConnection.Reconnect();
        }
    }
}