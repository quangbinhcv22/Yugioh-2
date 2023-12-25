using System;
using Newtonsoft.Json.Linq;
using QBPlugins.ScreenFlow;
using UnityEngine;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static PlayerInfo_Server playerInfo;

            public class PlayerInfo_Server
            {
                public int gold;
                public int level;
                public string displayName;
                public string avatarImage;
                public string coverImage;
                public string rank;
                public long id;
                public int exp;
                public string username;
                public int ruby;

                public PlayerInfo_Server(Response_LoginByPassword data)
                {
                    gold = data.gold;
                    level = data.level;
                    displayName = data.displayName;
                    avatarImage = data.avatarImage;
                    coverImage = data.coverImage;
                    rank = data.rank;
                    id = data.id;
                    exp = data.exp;
                    username = data.username;
                    ruby = data.ruby;
                }
            }


            public static Request_LoginByPassword cachedLoginRequest;
            public static Request_LoginByPassword _tempCachedLoginRequest;
        }

        public static partial class Request
        {
            public static void LoginByPassword(Request_LoginByPassword data)
            {
                Cached.cachedLoginRequest = data;
                Send(MessageID.LOGIN_BY_PASSWORD, data);
            }
        }

        public static partial class HandleResponse
        {
            public static void LoginByPassword(JObject data, string error)
            {
                var haveError = !string.IsNullOrEmpty(error);

                if (NetworkConnection.IsReconnecting && haveError)
                {
                    NetworkConnection.Reconnect();
                    return;
                }

                if (string.IsNullOrEmpty(error))
                {
                    var response = data.ToObject<Response_LoginByPassword>();

                    // Cached.cachedLoginRequest = Cached._tempCachedLoginRequest;


                    Cached.playerInfo = new Cached.PlayerInfo_Server(response);

                    if (NetworkConnection.IsReconnecting)
                    {
                        NetworkConnection.IsReconnecting = false;
                        Screen_Reconnecting.singleton.Close();
                    }
                    else
                    {
                        Request.GetMyDecks();
                        Request.Config.GetCardDataVersion();
                    }

                    if (ScreenManager.CurrentWindow.name.Contains(ScreenKey.Login))
                    {
                        ScreenManager.Open(ScreenKey.Loading);
                    }

                    Request.ReconnectGameSession();
                }
                else
                {
                    Popup_ErrorNotify.Open("Can not login", error);
                }
            }
        }
    }

    [Serializable]
    public struct Request_LoginByPassword
    {
        public string password;
        public string username;
    }

    [Serializable]
    public struct Response_LoginByPassword
    {
        public int gold;
        public int level;
        public string displayName;
        public string avatarImage;
        public string coverImage;
        public string rank;
        public long id;
        public int exp;
        public string username;
        public int ruby;
    }
}