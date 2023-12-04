namespace battle.define
{
    public enum MonsterAttribute
    {
        Fire = 1,
        Water = 2,
        Wind = 3,
        Earth = 4,
        Light = 5,
        Dark = 6,
        Divine = 7
    }

    public static class MonsterAttributeExtension
    {
        public static string IconKey(this MonsterAttribute attribute)
        {
            return $"attribute_{attribute.ToString().ToLower()}";
        }
    }
}