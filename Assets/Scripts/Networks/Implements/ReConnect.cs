using System.Collections.Generic;
using gameplay.manager;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static Request.LoadGameStateReason loadGameStateReason;
        }
        
        public static partial class Request
        {
            public static void ReconnectGameSession()
            {
                Send(MessageID.RECONNECT_GAME_SESSION);
            }

            public static void LoadGameState(LoadGameStateReason reason = LoadGameStateReason.Reconnect)
            {
                Cached.loadGameStateReason = reason;
                Send(MessageID.LOAD_GAME_STATE);
            }

            public enum LoadGameStateReason
            {
                Reconnect,
                UpdateEffect,
            }
        }

        public static partial class HandleResponse
        {
            public static void ReconnectGameSession(JObject data, string error)
            {
                if (!string.IsNullOrEmpty(error)) return;

                var response = data.ToObject<Response_ReconnectGameSession>();

                if (response.status == "SUCCESS")
                {
                    Request.LoadGameState();
                }
            }

            public static void LoadGameState(JObject data)
            {
                var response = data.ToObject<Response_LoadGameState>();

                if (Server_DueManager.main == null)
                {
                    DueManager.ReloadBattle(response);
                }
                else
                {
                    DueManager.Sync_GameState(response);
                }
            }
        }
    }


    public struct Response_ReconnectGameSession
    {
        public string status;
    }

    public struct Response_LoadGameState
    {
        public int gameId;
        public long myPlayerId;

        public int myHealthPoint;
        public int opponentHealthPoint;

        public int myRemainNumberDeckCards;
        public int opponentRemainNumberDeckCards;


        public string activePhase;
        public long turnedPlayerId;


        public List<SyncTemp_HandCard> myHandCards;
        public int opponentNumberHandCards;

        public List<SyncTemp_TableCard> myTableCards;
        public List<SyncTemp_TableCard> opponentTableCards;

        public SyncTemp_TableCard myFieldCard;
        public SyncTemp_TableCard opponentFieldCard;
    }


    public class SyncTemp_TableCard : SyncTemp_HandCard
    {
        public string face;
        public string position;
    }

    public class SyncTemp_HandCard
    {
        public string code;

        public int atk;
        public int def;

        public int effectedDEF;
        public int effectedATK;

        public long id;
        public string type;


        public ServerCard To_ServerCard()
        {
            return new ServerCard()
            {
                code = code,
                atk = atk,
                def = def,
                id = id,
            };
        }
    }
}