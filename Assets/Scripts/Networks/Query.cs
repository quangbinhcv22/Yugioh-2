using System.Collections.Generic;
using System.Linq;
using battle.define;
using Newtonsoft.Json;
using UnityEngine;

namespace Networks
{
    public static partial class Network
    {
        public static class Query
        {
            public static bool IsSelf(string playerID)
            {
                var selfID = Cached.playerInfo.id.ToString();
                return playerID == selfID;
            }

            public static bool IsSelf(long playerID)
            {
                var selfID = Cached.playerInfo.id.ToString();
                return playerID.ToString() == selfID;
            }

            public static Team ToTeam(object playerID)
            {
                var selfID = Cached.playerInfo.id.ToString();
                return playerID.ToString() == selfID ? Team.Self : Team.Opponent;
            }


            public static class Config
            {
                private static Dictionary<string, CardConfig> _cards;

                private static void LoadCards()
                {
                    if (_cards == null)
                    {
                        var cardDataKey = HandleResponse.Config.CardDataKey;
                        var json = PlayerPrefs.GetString(cardDataKey);

                        var cards = JsonConvert.DeserializeObject<List<CardConfig>>(json);
                        _cards = cards.ToDictionary(c => c.code, c => c);
                    }
                }

                public static CardConfig GetCard(string code)
                {
                    LoadCards();

                    if (DueCardQuery.IsAnonymousCard(code)) return new();

                    if (!_cards.ContainsKey(code))
                    {
                        return new();
                    }

                    return _cards[code];
                }

                public static bool IsValidCode(string code)
                {
                    LoadCards();
                    return _cards.ContainsKey(code);
                }

                public static List<CardConfig> Get_CardByType(CardType type)
                {
                    LoadCards();
                    return _cards.Values.Where(c => c.type == type.ToString().ToUpper()).ToList();
                }


                public static CardType GetType_ByCode(string code)
                {
                    switch (GetCard(code).type)
                    {
                        case "MONSTER":
                            return CardType.Monster;
                        case "SPELL":
                            return CardType.Spell;
                        default:
                            throw new KeyNotFoundException();
                    }
                }
            }


            public static class Fighting
            {
                public static Phase CurrentPhase
                {
                    get
                    {
                        var phaseData = Cached.Fighting.phase;

                        return phaseData.phase switch
                        {
                            "DRAW_PHASE" => Phase.Draw,
                            "STANDBY_PHASE" => Phase.Standby,
                            "MAIN_PHASE_1" => Phase.Main1,
                            "BATTLE_PHASE" => Phase.Battle,
                            "MAIN_PHASE_2" => Phase.Main2,
                            "END_PHASE" => Phase.End,
                            _ => throw new KeyNotFoundException()
                        };
                    }
                }

                public static bool IsOwnTurn
                {
                    get
                    {
                        var roundData = Cached.Fighting.round;
                        return IsSelf(roundData.player);
                    }
                }

                public static bool IsFirstTun => Cached.Fighting.turnCount < 1;

                public static Cached.PlayerInfo_Server GetTeam(Team team)
                {
                    return team == Team.Self ? Cached.matching_self : Cached.matching_opponent;
                }


                public static bool IsRevealedCard(string cardGuid)
                {
                    return Cached.Fighting.revealedCards.Contains(cardGuid);
                }

            }
        }
    }
}