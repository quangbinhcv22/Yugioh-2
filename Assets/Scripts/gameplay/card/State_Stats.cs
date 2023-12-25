using System;
using System.Collections.Generic;
using Networks;

namespace Gameplay.card
{
    public static class State_Stats
    {
        public static List<CardEffect> buffs = new();

        public static Action<CardEffect> onChanged;
        // public static Action<CardEffect> onRemove;

        public static Dictionary<string, int> buffsAtk = new();
        public static Dictionary<string, int> buffsDef = new();

        public static void Sync(Response_LoadGameState gameState)
        {
            var tableCards = new List<SyncTemp_TableCard>();
            tableCards.AddRange(gameState.myTableCards);
            tableCards.AddRange(gameState.opponentTableCards);
            
            
            foreach (var tableCard in tableCards)
            {
                var cardId = tableCard.id.ToString();

                if (!buffsAtk.ContainsKey(cardId))
                {
                    buffsAtk.Add(cardId, tableCard.effectedATK);
                }
                else
                {
                    buffsAtk[cardId] = tableCard.effectedATK;
                }
                
                if (!buffsDef.ContainsKey(cardId))
                {
                    buffsDef.Add(cardId, tableCard.effectedDEF);
                }
                else
                {
                    buffsDef[cardId] = tableCard.effectedDEF;
                }
            }
            
            onChanged?.Invoke(new());
        }


        public static void StartGame()
        {
            buffs = new();
            buffsAtk = new();
            buffsDef = new();
        }


        public static void Add(CardEffect buff)
        {
            buffs.Add(buff);
            onChanged?.Invoke(buff);
        }

        public static void Restore(CardEffect effect)
        {
            buffs.RemoveAll(buff => buff.cardId == effect.cardId);
            onChanged?.Invoke(effect);
        }

        // public static void Remove(CardEffect buff)
        // {
        //     buffs.Remove(buff);
        //     onRemove?.Invoke(buff);
        // }

        public static BuffState BuffState_Attack(string guid)
        {
            if (!buffsAtk.ContainsKey(guid))
            {
                return BuffState.Normal;
            }

            var buffAtk = buffsAtk[guid];

            // var buffAtk = 0;
            //
            //
            //
            // foreach (var buff in buffs)
            // {
            //     if (buff.cardId.ToString() != guid) continue;
            //     if (buff.type != CardEffect.Type.UPDATE_ATK) continue;
            //
            //     buffAtk += buff.value;
            // }

            if (buffAtk == 0) return BuffState.Normal;
            else if (buffAtk > 0) return BuffState.Buff;
            return BuffState.DeBuff;
        }

        public static BuffState BuffState_Defense(string guid)
        {
            if (!buffsDef.ContainsKey(guid))
            {
                return BuffState.Normal;
            }

            var buffDef = buffsDef[guid];

            // foreach (var buff in buffs)
            // {
            //     if (buff.cardId.ToString() != guid) continue;
            //     if (buff.type != CardEffect.Type.UPDATE_DEF) continue;
            //
            //     buffDef += buff.value;
            // }

            if (buffDef == 0) return BuffState.Normal;
            else if (buffDef > 0) return BuffState.Buff;
            return BuffState.DeBuff;
        }


        public static int AttackOf(string guid)
        {
            var buffAtk = buffsAtk.ContainsKey(guid) ? buffsAtk[guid] : 0;
            var config = DueCardQuery.GetConfig(guid);

            var atk = config.atk;
            atk += buffAtk;

            // foreach (var buff in buffs)
            // {
            //     if (buff.cardId.ToString() != guid) continue;
            //     if (buff.type != CardEffect.Type.UPDATE_DEF) continue;
            //
            //     atk += buff.value;
            // }

            return atk;
        }

        public static int DefenseOf(string guid)
        {
            var buffDef = buffsDef.ContainsKey(guid) ? buffsDef[guid] : 0;
            var config = DueCardQuery.GetConfig(guid);

            var def = config.def;
            def += buffDef;

            // foreach (var buff in buffs)
            // {
            //     if (buff.cardId.ToString() != guid) continue;
            //     if (buff.type != CardEffect.Type.UPDATE_DEF) continue;
            //
            //     def += buff.value;
            // }

            return def;
        }

        // public static int AttackOf(string guid)
        // {
        //     var location = DueCardQuery.Locate(guid);
        //     var team = location.OfTeam;
        //
        //     var info = DueCardQuery.GetViewInfo(guid) as ViewInfo_MonsterCard;
        //
        //     var buffs = GetBuffs(team);
        //     var attribute = info.attribute;
        //     var types = info.types;
        //
        //     var attributeBuff = 0;
        //     var typesBuff = 0;
        //
        //     foreach (var buff in buffs)
        //     {
        //         if (buff.filterAttributes.Contains(attribute)) attributeBuff += buff.atkNumber;
        //         // if (types.) attributeBuff += buff.number;
        //     }
        //
        //     var finalAtk = info.attack + attributeBuff + typesBuff;
        //     return finalAtk;
        // }

        // public static int DefOf(string guid)
        // {
        //     var location = DueCardQuery.Locate(guid);
        //     var team = location.OfTeam;
        //
        //     var info = DueCardQuery.GetViewInfo(guid) as ViewInfo_MonsterCard;
        //
        //     var buffs = GetBuffs(team);
        //     var attribute = info.attribute;
        //     var types = info.types;
        //
        //     var attributeBuff = 0;
        //     var typesBuff = 0;
        //
        //     foreach (var buff in buffs)
        //     {
        //         if (buff.filterAttributes.Contains(attribute)) attributeBuff += buff.defNumber;
        //         // if (types.) attributeBuff += buff.number;
        //     }
        //
        //     var finalAtk = info.defense + attributeBuff + typesBuff;
        //     return finalAtk;
        // }
    }

    public enum BuffState
    {
        Normal,
        Buff,
        DeBuff,
    }
}