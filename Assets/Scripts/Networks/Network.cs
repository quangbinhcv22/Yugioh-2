using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QBPlugins.ScreenFlow;
using UnityEngine;

namespace Networks
{
    public static partial class Network
    {
        public static void Send(string id, object request = null)
        {
            object requestMessage =
                request == null ? new Message_OnlyID { id = id } : new MessageRequest { id = id, data = request, };

            var message = JsonConvert.SerializeObject(requestMessage);
            Debug.Log($"<color=cyan>Request:</color> {message}");

            NetworkConnection.ws.SendText(message);
        }

        public static void SimulateMessage(object data)
        {
            var message = JsonConvert.SerializeObject(data);
            OnMessage(message, true);
        }

        public static void OnMessage(string message, bool fakeMessage = false)
        {
            if (fakeMessage)
            {
                Debug.Log($"<color=green>Client-Response:</color> {message}");
            }
            else
            {
                Debug.Log($"<color=yellow>Response:</color> {message}");
            }
            
            var msgObject = JsonConvert.DeserializeObject<MessageResponse>(message);
            

            var data = msgObject.data;

            if (!string.IsNullOrEmpty(msgObject.error))
            {
                Popup_ErrorNotify.Open($"Server Eror: {msgObject.id}", content: $"{msgObject.error}");
            }

            switch (msgObject.id)
            {
                case MessageID.LOGIN_BY_PASSWORD:
                    HandleResponse.LoginByPassword((JObject)data, msgObject.error);
                    break;
                case MessageID.GET_MY_DECKS:
                    HandleResponse.GetMyDecks((JArray)data);
                    break;


                case MessageID.GET_CARD_DATA_VERSION:
                    HandleResponse.Config.GetCardDataVersion((string)data);
                    break;
                case MessageID.GET_CARD_DATA:
                    HandleResponse.Config.GetCardData((JArray)data);
                    break;


                case MessageID.FIND_MATCH:
                    HandleResponse.FindMatch((JObject)data);
                    break;
                case MessageID.FIND_MATCH_CANCEL:
                    HandleResponse.FindMachCancel((JObject)data);
                    break;

                case MessageID.MATCHING_ROOM_INIT:
                    HandleResponse.MatchRoomInit((JObject)data);
                    break;

                case MessageID.MATCHING_ROOM_CONFIRM:
                    HandleResponse.MatchRoomConfirm((JObject)data);
                    break;
                case MessageID.MATCHING_ROOM_CANCEL:
                    HandleResponse.MatchingRoomCancel((JObject)data);
                    break;

                case MessageID.START_ORDER_TO_GO:
                    HandleResponse.Matching_StartOrderToGo((JObject)data);
                    break;
                case MessageID.SELECT_ORDER_TO_GO:
                    HandleResponse.Matching_SelectOrderToGo((JObject)data);
                    break;


                case MessageID.START_GAME:
                    HandleResponse.BattleStartGame((JObject)data);
                    break;
                case MessageID.START_ROUND:
                    HandleResponse.StartRound((JObject)data);
                    break;
                case MessageID.ON_PHASE:
                    HandleResponse.OnPhase((JObject)data);
                    break;
                case MessageID.DRAW_DECK_CARD:
                    HandleResponse.DrawCheckCard((JObject)data);
                    break;
                
                case MessageID.END_GAME:
                    HandleResponse.EndGame((JObject)data);
                    break;

                case MessageID.RELEASE_HAND_CARD:
                    HandleResponse.Fighting.ReleaseHandCard((JObject)data);
                    break;
                
                case MessageID.ATTACK_TABLE_DIRECT:
                    HandleResponse.Fighting.AttackTableDirect((JObject)data);
                    break;
                case MessageID.ATTACK_TABLE_CARD:
                    HandleResponse.Fighting.AttackTableCard((JObject)data);
                    break;


                case MessageID.DISCONNECT:
                    HandleResponse.Disconnect((JObject)data);
                    break;
                // case MessageID.MATCHING_ROOM_READY_START_GAME:
                //     HandleResponse.MatchRoomInit((JObject)data);
                //     break;
            }
        }
    }
}