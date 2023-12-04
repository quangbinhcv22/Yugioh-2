using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static Response_MatchRoomInit matchRoomInit;
            public static Response_StartOrderToGo matching_startOrderToGo;

            public static bool matching_selfConfirm;
            public static bool matching_opponentConfirm;

            public static PlayerInfo_Server matching_self;
            public static PlayerInfo_Server matching_opponent;

            public static long matching_playerSelectTurn;
            public static long matching_myTurn;

            public static void ResetMatching()
            {
                matchRoomInit = default;
                matching_startOrderToGo = default;
                
                matching_selfConfirm = default;
                matching_opponentConfirm = default;
                
                matching_self = default;
                matching_opponent = default;
                
                matching_playerSelectTurn = default;
                matching_myTurn = default;
            }
        }

        public static partial class Event
        {
            public static Action onMatchingSuccess;
            public static Action onMatchingCancel;

            public static Action onRoomInit;
            public static Action onMatchingConfirm;


            public static Action matching_StartOrderToGo;
            public static Action matching_SelectOrderToGo;
        }

        public static partial class Request
        {
            public static void FindMatchDefault()
            {
                FindMatch(new Request_FindMatch()
                {
                    deckId = Cached.decks.First().id,
                    type = "STANDARD",
                });
            }

            public static void FindMatch(Request_FindMatch data)
            {
                Cached.ResetMatching();

                Networks.Network.Send(MessageID.FIND_MATCH, data);
            }

            public static void ConfirmMatching()
            {
                Send(MessageID.MATCHING_ROOM_CONFIRM);
            }

            public static void Matching_ReadyStartGame()
            {
                Send(MessageID.MATCHING_ROOM_READY_START_GAME);
            }

            public static void Matching_SelectOrderToGo(Request_SelectOrderToGo data)
            {
                Send(MessageID.SELECT_ORDER_TO_GO, data);
            }
        }

        public static partial class HandleResponse
        {
            public static void FindMatch(JObject data)
            {
                var response = data.ToObject<Response_FindMatch>();
                // if(response.status != "SUCCESS") return;

                Event.onMatchingSuccess?.Invoke();
            }

            public static void FindMachCancel(JObject data)
            {
                var response = data.ToObject<Response_FindMatchCancel>();
                Popup_ErrorNotify.Open("Find match cancel", response.reason);

                Event.onMatchingCancel?.Invoke();
            }


            public static void MatchRoomInit(JObject data)
            {
                var response = data.ToObject<Response_MatchRoomInit>();
                Cached.matchRoomInit = response;

                foreach (var player in response.players)
                {
                    if (player.id == Cached.playerInfo.id)
                    {
                        Cached.matching_self = player;
                    }
                    else
                    {
                        Cached.matching_opponent = player;
                    }
                }


                Event.onRoomInit?.Invoke();
            }

            public static void MatchRoomConfirm(JObject data)
            {
                var response = data.ToObject<Response_MatchRoomConfirm>();

                if (response.playerId == Cached.matching_self.id.ToString())
                {
                    Cached.matching_selfConfirm = true;
                }

                if (response.playerId == Cached.matching_opponent.id.ToString())
                {
                    Cached.matching_opponentConfirm = true;
                }

                Event.onMatchingConfirm?.Invoke();

                if (Cached.matching_selfConfirm && Cached.matching_opponentConfirm)
                {
                    Request.Matching_ReadyStartGame();
                }
            }

            public static void MatchingRoomCancel(JObject data)
            {
                var response = data.ToObject<Response_MatchingRoomCancel>();
                
                Popup_ErrorNotify.Open("Matching room cancel", response.reason);


                if (response.reason == "PLAYER_DISCONNECT")
                {
                    Popup_Disconnect.GotoLogin();
                }
                else
                {
                    Event.onMatchingCancel?.Invoke();
                }
            }


            public static void Matching_StartOrderToGo(JObject data)
            {
                var response = data.ToObject<Response_StartOrderToGo>();
                Cached.matching_startOrderToGo = response;

                Cached.matching_playerSelectTurn = response.player;

                Event.matching_StartOrderToGo?.Invoke();
            }

            public static void Matching_SelectOrderToGo(JObject data)
            {
                var response = data.ToObject<Response_SelectOrderToGo>();

                var isSelf = response.player == Cached.playerInfo.id;
                if (isSelf) Cached.matching_myTurn = response.order;
                else Cached.matching_myTurn = response.order == 1 ? 2 : 1;

                Event.matching_SelectOrderToGo?.Invoke();
            }
        }
    }

    public struct Request_FindMatch
    {
        public long deckId;
        public string type;
    }

    public struct Response_FindMatch
    {
        public string status;
    }

    public class Response_FindMatchCancel
    {
        public string reason;
    }

    public struct Response_MatchRoomInit
    {
        public int stateTimeout;
        public List<Network.Cached.PlayerInfo_Server> players;
    }

    public struct Response_MatchRoomConfirm
    {
        public string status;
        public string playerId;
    }
    
    public class Response_MatchingRoomCancel
    {
        public string reason;
    }

    public struct Response_MatchRoomReadyStartGame
    {
        public string playerId;
    }


    public struct Response_StartOrderToGo
    {
        public long player;
        public int timeout;
    }

    public struct Request_SelectOrderToGo
    {
        public int order;
    }

    public struct Response_SelectOrderToGo
    {
        public long player;
        public int order;
    }
}