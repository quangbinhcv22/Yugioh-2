using System;
using System.Collections.Generic;
using battle.define;
using Gameplay.card.core;

namespace Gameplay.card
{
// {
//     public class CardEffect
//     {
//         public EffectCondition condition;
//         public EffectCost cost;
//         public EffectAction action;
//     }
    

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
            // State_Stats.Add(team, buff);
        }

        public void OnDeactivate()
        {
            // State_Stats.Remove(team, buff);
        }
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