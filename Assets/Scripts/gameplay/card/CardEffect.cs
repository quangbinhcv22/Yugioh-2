using System;
using System.Collections.Generic;
using battle.define;
using Gameplay.card.core;

namespace Gameplay.card
{
    public class CardEffect
    {
        public EffectCondition condition;
        public EffectCost cost;
        public EffectAction action;
    }
    

    public class EffectAction
    {
    }

    public class EffectCost
    {
    }

    public class EffectCondition
    {
    }

    public interface IEffectCondition
    {
    }

    public class Effect_BuffAttack
    {
        public BuffStat buff;
        public Team team;

        public void OnAction()
        {
            State_Stats.Add(team, buff);
        }

        public void OnDeactivate()
        {
            State_Stats.Remove(team, buff);
        }
    }

    public static class State_Stats
    {
        public static List<BuffStat> self = new();
        public static List<BuffStat> opponent = new();

        public static Action<Team, BuffStat> onAdd;
        public static Action<Team, BuffStat> onRemove;


        public static void StartGame()
        {
            self = new();
            opponent = new();
        }

        public static void Reset()
        {
            self.Clear();
            opponent.Clear();
        }


        public static void Add(Team team, BuffStat buff)
        {
            var currently = GetBuffs(team);
            currently.Add(buff);

            onAdd?.Invoke(team, buff);
        }

        public static void Remove(Team team, BuffStat buff)
        {
            var currently = GetBuffs(team);
            currently.Remove(buff);

            onRemove?.Invoke(team, buff);
        }

        public static List<BuffStat> GetBuffs(Team team)
        {
            return team is Team.Self ? self : opponent;
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

    public class BuffStat
    {
        public int atkNumber;
        public int defNumber;
        public List<MonsterAttribute> filterAttributes;
        public MonsterTypes filterTypes;
        
        public Func<int> atkCustom;
        public Func<int> defCustom;
    }
}