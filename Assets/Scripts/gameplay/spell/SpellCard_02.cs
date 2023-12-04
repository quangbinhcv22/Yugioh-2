using System.Collections.Generic;
using battle.define;

namespace QBPlugins.Game.Core
{
    [Factory("sp", "sp02")]
    public class SpellCard_02 : SpellCard_BuffStat
    {
        public SpellCard_02()
        {
            buff = new()
            {
                atkNumber = 400,
                defNumber = -200,
                filterAttributes = new List<MonsterAttribute>() { MonsterAttribute.Wind },
            };
        }
    }
}