using System.Collections.Generic;
using battle.define;
using Gameplay.card;

namespace QBPlugins.Game.Core
{
    public class SpellCard_BuffStat : SpellCard
    {
        public BuffStat buff;
        
        public override void OnActive()
        {
            State_Stats.Add(team, buff);
        }

        public override void OnDeactivate()
        {
            State_Stats.Remove(team, buff);
        }
    }


    [Factory("sp", "sp01")]
    public class SpellCard_01 : SpellCard_BuffStat
    {
        public SpellCard_01()
        {
            buff = new()
            {
                atkNumber = 700,
                filterAttributes = new List<MonsterAttribute>() { MonsterAttribute.Fire, MonsterAttribute.Dark, MonsterAttribute.Divine, MonsterAttribute.Earth, MonsterAttribute.Light, MonsterAttribute.Water, MonsterAttribute.Wind },
            };
        }
    }

    [Factory("sp", "sp04")]
    public class SpellCard_04 : SpellCard_BuffStat
    {
        public SpellCard_04()
        {
            buff = new()
            {
                atkNumber = 300,
                defNumber = 300,
                filterTypes = MonsterTypes.Beast,
            };
        }
    }
}