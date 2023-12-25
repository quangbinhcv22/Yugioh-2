using System.Collections.Generic;
using Gameplay.board;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static partial class Fighting
            {
                public static List<string> revealedCards = new();
            }
        }


        public static partial class Event
        {
            public static partial class Fighting
            {
                // public static Action<Event_RevealedCard> event_RevealedCard;

                // public static void RevealedCard(Event_RevealedCard changed)
                // {
                //     var card = DueCardQuery.GetData(changed.cardId);
                //     card.code = @changed.cardCode;
                //     
                //     DueCardQuery.InitCard(card);
                //
                //     if (!Cached.Fighting.revealedCards.Contains(changed.cardId))
                //     {
                //         Cached.Fighting.revealedCards.Add(changed.cardId);      
                //      
                //         DueCardQuery.GetUICard_CombatSpace(card.Guid).OnRevealedCard(changed);
                //
                //         // event_RevealedCard?.Invoke(changed);
                //     }
                // }
            }
        }


        public static partial class Request
        {
            public static partial class Fighting
            {
                public static void ChangeTableCardPosition(Request_ChangeTableCardPosition request)
                {
                    Send(MessageID.CHANGE_TABLE_CARD_POSITION, request);
                }
            }
        }

        public static partial class HandleResponse
        {
            public static partial class Fighting
            {
                public static void ChangeTableCardPosition(JObject data)
                {
                    var response = data.ToObject<Response_ChangeTableCardPosition>();

                    var player = Server_DueManager.main.GetPlayer(response.player.ToString());

                    // var monster

                    var cardId = response.card.id.ToString();

                    var monster = player.zone.mainMonster.Get(cardId).card;
                    if(string.IsNullOrEmpty(monster.code)) monster.code = response.card.code;

                    if (response.position == MonsterPosition.Defense.ServerKey())
                    {
                        CardAction_PhaseMain.Current.Notify_WasAttacker(response.card.id.ToString());
                    }
                    // var a = DueCardQuery.GetData(monster.Guid);

                    var newPosition = player.zone.mainMonster.ChangePosition(cardId);

                    Notifier_DueData.Current.Event_ChangePosition(new()
                    {
                        team = Query.ToTeam(response.player),
                        guid = cardId,
                        position = newPosition,
                    });
                    
                    
                    // Event.Fighting.RevealedCard(new() { cardId = cardId, cardCode = response.card.code});
                }
            }
        }
    }

    public struct Request_ChangeTableCardPosition
    {
        public long cardId;
        public string position;
    }

    public struct Response_ChangeTableCardPosition
    {
        public long player;
        public string position;
        public ServerCard card;
    }

    public struct Event_RevealedCard
    {
        public string cardId;
        public string cardCode;
    }
}