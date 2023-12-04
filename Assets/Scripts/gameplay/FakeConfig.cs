using System;
using System.Collections.Generic;
using battle.define;
using CardConfig = Networks.CardConfig;

namespace Gameplay
{
    public static class FakeConfig
    {
        public static Dictionary<string, CardConfig> monsters;
        public static Dictionary<string, CardConfig> spells;


        static FakeConfig()
        {
            // spells = SpellCard_Query.defines.ToDictionary(d => d.id, d => d);
        }
        

        public static string GetIllusKey(string id)
        {
            var type = GetType_ById(id);
            switch (type)
            {
                case CardType.None:
                    break;
                case CardType.Monster:
                    return $"{id}";
                    break;
                case CardType.Spell:
                    return id;
                    break;
                case CardType.Trap:
                    break;
                case CardType.Field:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return string.Empty;
        }

        public static CardType GetType_ByGuid(string guid)
        {
            var id = DueCardQuery.GetViewInfo(guid).code;
            return GetType_ById(id);
        }
        
        public static CardType GetType_ById(string id)
        {
            if (id.Contains("sp")) return CardType.Spell;
            return CardType.Monster;
        }


        public static int GetAttackBase(string id)
        {
            return monsters[id].atk;
        }

        public static int GetDefenseBase(string id)
        {
            return monsters[id].def;
        }

        // public static void NewMonster()
        // {
        //     monsters = new();
        //
        //
        //     var configs = Resources.Load<ConfigMonsters>("config_monsters");
        //
        //     foreach (var config in configs.configs)
        //     {
        //         monsters.Add(config.id, ViewInfo_MonsterCard.GetFake(config.id));
        //     }
        // }

        // public static List<string> RandomMain()
        // {
        //     const int maxSame = 3;
        //     var ids = new List<string>();
        //
        //
        //     for (int i = 0; i < 40; i++)
        //     {
        //         var weight = Random.Range(0, 100);
        //
        //         // if (weight > 45)
        //         // {
        //         //     var id = SpellCard_Query.GetRandom();
        //         //     ids.Add(id);
        //         // }
        //         // else
        //         {
        //             var id = RandomMonsterId;
        //             if (ids.Count(ie => ie == id) >= maxSame) i--;
        //             else ids.Add(id);
        //         }
        //     }
        //
        //     return ids.OrderBy(_ => Random.Range(0, 1f)).ToList();
        // }

        // public static string RandomMonsterId
        // {
        //     get
        //     {
        //         return monsters.ToList()[Random.Range(0, monsters.Count)].Key;
        //     }
        // }
    }
}