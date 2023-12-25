namespace Gameplay.board
{
    public enum MonsterPosition
    {
        Unset = -1,
        Standard = 0,
        Attack = 1,
        Defense = 2
    }

    public static class MonsterPositionExtension
    {
        public static string ServerKey(this MonsterPosition position)
        {
            return position.ToString().ToUpper();
        }
        
        public static MonsterPosition ChangeCase(this MonsterPosition position)
        {
            return position is MonsterPosition.Attack ? MonsterPosition.Defense : MonsterPosition.Attack;
        }
    }
}