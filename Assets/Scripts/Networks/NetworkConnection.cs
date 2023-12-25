using System.Text;
using NativeWebSocket;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Networks
{
    public class NetworkConnection : MonoBehaviour
    {
        [ShowInInspector] public static WebSocket ws;
        [ShowInInspector] public static string messageToSend;

        [Button]
        private void Send()
        {
            Network.Send(messageToSend);
        }


        // public b

        public static async void Connect()
        {
            ws = new WebSocket("wss://game.yugimaster.com/socket.io");

            ws.OnOpen += WsOnOpen;
            ws.OnError += WsOnError;
            ws.OnClose += WsOnClose;
            ws.OnMessage += WsOnMessage;

            Debug.Log("Connecting...");
            await ws.Connect();
        }


        public static bool IsReconnecting { get; set; }

        public static async void Reconnect()
        {
            Debug.Log("Reconnect...");
            IsReconnecting = true;
            Connect();
        }


        // Start is called before the first frame update
        async void Start()
        {
            Connect();
        }


        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws == null) return;
            ws.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit()
        {
            if (ws != null) await ws.Close();
        }


        private static void WsOnOpen()
        {
            Debug.Log("WS Open");

            if (IsReconnecting)
            {
                Network.Request.LoginByPassword(Network.Cached.cachedLoginRequest);
            }
        }

        private static void WsOnError(string errorMsg)
        {
            Debug.Log($"WS Error: {errorMsg}");
        }

        private static void WsOnClose(WebSocketCloseCode closeCode)
        {
            Debug.Log($"WS Close: {closeCode}");
        }

        private static void WsOnMessage(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            // Debug.Log($"WS Message: {message}");

            Network.OnMessage(message);
        }

        [Button]
        public static void CancelConnection()
        {
            ws.CancelConnection();
        }
    }
}