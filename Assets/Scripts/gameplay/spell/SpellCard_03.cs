using battle.define;

namespace QBPlugins.Game.Core
{
    [Factory("sp", "sp03")]
    public class SpellCard_03 : SpellCard_BuffStat
    {
        public SpellCard_03()
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