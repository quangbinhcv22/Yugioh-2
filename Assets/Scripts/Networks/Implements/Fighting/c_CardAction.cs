using System;
using battle.define;
using Gameplay.board;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Event
        {
            public static partial class Fighting
            {
                public static Action<Event_SetField> SetField;
                public static Action<Event_DestroyField> DestroyField;
            }
        }


        public static partial class Request
        {
            public static partial class Fighting
            {
                public static void ReleaseHandCard(Request_ReleaseHandCard data)
                {
                    Send(MessageID.RELEASE_HAND_CARD, data);
                }

                public static void ReleaseHandCard(Request_ReleaseHandCard_Spell data)
                {
                    Send(MessageID.RELEASE_HAND_CARD, data);
                }
            }
        }

        public static partial class HandleResponse
        {
            public static partial class Fighting
            {
                public static void ReleaseHandCard(JObject data)
                {
                    var response = data.ToObject<Response_ReleaseHandCard>();

                    var isSelf = Query.IsSelf(response.player);
                    var player = isSelf ? Server_DueManager.main.self : Server_DueManager.main.opponent;
                    var playerIndex = isSelf ? 0 : 1;


                    var card = player.zone.inHand.Release(response.card.Guid, response.card);
                    DueNotifier.Notify_InHandRemove(playerIndex, card.Guid);


                    // if (response.mode is ReleaseHandCardType.Summon)
                    // {
                    //     
                    // }

                    var cardCode = response.card.code ?? card.code;
                    var cardPosition = response.card.position;
                    
                    var isAnonymousCode = DueCardQuery.IsAnonymousCode(card.code);
                    
                    var cardConfig = Query.Config.GetCard(cardCode);
                    // var type = Query.Config.GetType_ByCode(response.card.code);

                    if (cardPosition == MonsterPosition.Attack.ServerKey() || cardPosition == MonsterPosition.Defense.ServerKey())
                    {
                        player.normalSummonThisTurn = true;

                        var position = response.mode == ReleaseHandCardType.Summon
                            ? MonsterPosition.Attack
                            : MonsterPosition.Defense;

                        var index = position is MonsterPosition.Attack
                            ? player.zone.mainMonster.Summon_Attack(card)
                            : player.zone.mainMonster.Summon_Defense(card);
                        
                        Notifier_DueData.Current.Event_SummonMonster(new()
                        {
                            playerIndex = playerIndex,
                            summonGuid = card.Guid,
                            position = position,
                            summonIndex = index,
                        });
                        
                        if (position is MonsterPosition.Attack)
                        {
                            // Event.Fighting.RevealedCard(new(){cardId = response.card.Guid, cardCode = response.card.code});
                        }
                    }
                    else if (response.mode == ReleaseHandCardType.Active)
                    {
                        if (cardConfig.spellType == SpellType.FIELD.ToString())
                        {
                            player.zone.field.Set(card);

                            var @event = new Event_SetField()
                            {
                                team = (Team) playerIndex,
                                serverCard = card,
                            };
                            
                            Event.Fighting.SetField?.Invoke(@event);
                        }
                        else
                        {
                            // var index = player.zone.spellTrap.Set(card);
                            //
                            // Notifier_DueData.Current.Event_SetSpell(new()
                            // {
                            //     team = playerIndex == 0 ? Team.Self : Team.Opponent,
                            //     guid = card.Guid,
                            //     index = index,
                            // });

                            // var spell = SpellCard_Query.InstantiateSpell(team, cardGuid);
                            // spell.OnActive();
                        }
                    }
                }
            }
        }
    }

    public struct Request_ReleaseHandCard
    {
        public long cardId;
        public string mode;
    }

    public static class ReleaseHandCardType
    {
        public const string Active = "ACTIVE";
        public const string Summon = "SUMMON";
        public const string Set = "SET";
    }

    public struct Request_ReleaseHandCard_Spell
    {
        public long cardId;
        public string mode;
    }


    public class Response_ReleaseHandCard
    {
        public long player;
        public ServerCard card;
        public string mode;
    }

    public class Event_SetField
    {
        public Team team;
        public ServerCard serverCard;
    }
    
    public class Event_DestroyField
    {
        public Team team;
        public string guid;
    }
}