using battle.define;
using Gameplay.card.@enum;
using UnityEngine;

namespace Gameplay.card.config
{
    [CreateAssetMenu(menuName = "Config/Card/Monster", fileName = "config_monster_", order = 0)]
    public class CardConfig_Monster : CardConfig
    {
        public int level;
        public MonsterAttribute attribute;
        public MonsterTypes types;
        public int attack;
        public int defense;
    }
}