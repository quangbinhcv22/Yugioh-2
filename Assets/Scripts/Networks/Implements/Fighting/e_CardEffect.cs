using System.Collections.Generic;
using battle.define;
using Gameplay.card;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Networks
{
    public class e_CardEffect
    {
    }

    public static partial class Network
    {
        public static partial class Cached
        {
            public static partial class Fighting
            {
                public static List<string> dieCards = new();

                public static void ResolveDieCard()
                {
                    dieCards.Clear();
                }
            }
        }

        public static partial class HandleResponse
        {
            public static void On_CardEffect(JObject data)
            {
                var response = data.ToObject<CardEffects>();

                foreach (var effect in response.effects)
                {
                    if (effect.type == CardEffect.Type.DESTROY)
                    {
                        // Server_DueManager.main.SendToGraveyard(effect.cardId.ToString());

                        if (Query.Config.GetType_ByCode(effect.cardCode) == CardType.Monster)
                        {
                            Server_DueManager.main.MonsterDie(effect.cardId.ToString());
                            Cached.Fighting.dieCards.Add(effect.cardId.ToString());
                        }

                        if (Query.Config.GetCard(effect.cardCode).spellType == SpellType.FIELD.ToString())
                        {
                            var location = DueCardQuery.Locate(effect.cardId.ToString());


                            // var a = Server_DueManager.main.player1.zone.field.space.card.Guid == effect.cardId.ToString();
                            // var b = Server_DueManager.main.player1.zone.field.space.card.Guid == effect.cardId.ToString();
                            //
                            // Debug.Log($"{location.zoneType} - {location.index} - {location.playerIndex}");
                            
                            Server_DueManager.main.FieldCardDie(effect.cardId.ToString());

                            // var team = location.OfTeam;
                            
                            Event.Fighting.DestroyField?.Invoke(new()
                            {
                                team = location.OfTeam,
                                guid = effect.cardId.ToString(),
                            });
                        }
                    }

                    if (effect.type is CardEffect.Type.UPDATE_ATK or CardEffect.Type.UPDATE_DEF)
                    {
                        State_Stats.Add(effect);
                    }

                    if (effect.type is CardEffect.Type.RESTORE_ATK_DEF)
                    {
                        State_Stats.Restore(effect);
                    }
                }
                
                Request.LoadGameState(Request.LoadGameStateReason.UpdateEffect);
            }
        }
    }

    public struct CardEffects
    {
        public List<CardEffect> effects;
    }

    public struct CardEffect
    {
        public long cardId;
        public string cardCode;

        public bool isActive;
        
        public string type;
        public int value;

        public static class Type
        {
            public const string UPDATE_ATK = "UPDATE_ATK";
            public const string UPDATE_DEF = "UPDATE_DEF";
            public const string DESTROY = "DESTROY";
            public const string RESTORE_ATK_DEF = "RESTORE_ATK_DEF";
        }
    }
}