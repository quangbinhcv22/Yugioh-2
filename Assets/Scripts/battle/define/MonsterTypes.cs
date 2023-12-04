using System;

namespace battle.define
{
    [Flags]
    public enum MonsterTypes
    {
        None = 0,
        Dragon = 1 << 0,
        Warrior = 1 << 1,
        Beast = 1 << 2,
        Pyro = 1 << 3,
        Machine = 1 << 4,
        Zombie = 1 << 5,
        Aqua = 1 << 6,
        Rock = 1 << 7,
        Fiend = 1 << 8,
        SpellCaster = 1 << 9,
        Insect = 1 << 10,
        Fairy = 1 << 11,
        Fish = 1 << 12,
        Reptile = 1 << 13,
        Thunder = 1 << 14,
        Dinosaur = 1 << 15,
        SeaSerpent = 1 << 16,
        WingedBeast = 1 << 17,
        BeastWarrior = 1 << 18,
        Plant = 1 << 19,
        DivineBeast = 1 << 20,
    }
    
    public static class MonsterTypesExtension
    {
        public static string ToText(this MonsterTypes types)
        {
            return $"[{types.ToString().Replace(", ", "/")}]";
        }
    }
}