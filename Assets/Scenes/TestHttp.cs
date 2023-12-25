using System;
using NativeWebSocket;
using Sirenix.OdinInspector;
using UnityEngine;
using Network = Networks.Network;

public class TestHttp : MonoBehaviour
{
    private WebSocket ws;


    // Start is called before the first frame update
    async void Start()
    {
        // ws = new WebSocket("wss://game-dev.yugimaster.com/socket.io");
        //
        // ws.OnOpen += WsOnOpen;
        // ws.OnError += WsOnError;
        // ws.OnClose += WsOnClose;
        // ws.OnMessage += WsOnMessage;
        //
        //
        // // Keep sending messages at every 0.3s
        // // InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
        //
        // Debug.Log("Connecting...");
        // await ws.Connect();
    }


    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // ws.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        if (ws != null) await ws.Close();
    }


    private void WsOnOpen()
    {
        Debug.Log("WS Open");
    }

    private void WsOnError(string errorMsg)
    {
        Debug.Log($"WS Error: {errorMsg}");
    }

    private void WsOnClose(WebSocketCloseCode closeCode)
    {
        Debug.Log($"WS Close: {closeCode}");
    }

    private void WsOnMessage(byte[] data)
    {
        Debug.Log($"WS Message: {data}");
    }

    [Button]
    public void FakeResponse(string message)
    {
        Network.OnMessage(message);
        // Debug.Log($"<color=yellow>Response:</color> {message}");
    }
}


public static class MessageID
{
    public const string GET_CARD_DATA_VERSION = "GET_CARD_DATA_VERSION";
    public const string GET_CARD_DATA = "GET_CARD_DATA";
    
    public const string LOGIN_BY_PASSWORD = "LOGIN_BY_PASSWORD";
    public const string GET_MY_DECKS = "GET_MY_DECKS";


    public const string FIND_MATCH = "FIND_MATCH";
    public const string FIND_MATCH_CANCEL = "FIND_MATCH_CANCEL";

    public const string MATCHING_ROOM_INIT = "MATCHING_ROOM_INIT";

    public const string MATCHING_ROOM_CONFIRM = "MATCHING_ROOM_CONFIRM";
    public const string MATCHING_ROOM_CANCEL = "MATCHING_ROOM_CANCEL";

    public const string MATCHING_ROOM_READY_START_GAME = "MATCHING_ROOM_READY_START_GAME";

    public const string START_ORDER_TO_GO = "START_ORDER_TO_GO";
    public const string SELECT_ORDER_TO_GO = "SELECT_ORDER_TO_GO";


    public const string TESTING_SET_CARD_DECK_BEFORE_START_GAME = "TESTING_SET_CARD_DECK_BEFORE_START_GAME";
    public const string TESTING_END_ALL_GAME_SESSIONS = "TESTING_END_ALL_GAME_SESSIONS";
    
    public const string START_GAME = "START_GAME";
    public const string START_ROUND = "START_ROUND";
    public const string ON_PHASE = "ON_PHASE";
    public const string DRAW_DECK_CARD = "DRAW_DECK_CARD";
    
    public const string ATTACK_TABLE_CARD = "ATTACK_TABLE_CARD";
    public const string ATTACK_TABLE_DIRECT = "ATTACK_TABLE_DIRECT";
    public const string CHANGE_TABLE_CARD_POSITION = "CHANGE_TABLE_CARD_POSITION";
    
    public const string CARD_EFFECT = "CARD_EFFECT";


    public const string DISCONNECT = "DISCONNECT";
    
    public const string ACTIVE_BATTLE_PHASE = "ACTIVE_BATTLE_PHASE";
    public const string END_PHASE = "END_PHASE";
    public const string RELEASE_HAND_CARD = "RELEASE_HAND_CARD";
    
    public const string RECONNECT_GAME_SESSION = "RECONNECT_GAME_SESSION";
    public const string LOAD_GAME_STATE = "LOAD_GAME_STATE";
    
    public const string END_GAME = "END_GAME";
}


[Serializable]
public struct MessageRequest
{
    public string id;
    public object data;
}

[Serializable]
public struct MessageResponse
{
    public string id;
    public object data;
    public string error;
}

[Serializable]
public struct Message_OnlyID
{
    public string id;
}