namespace battle.define
{
    public enum Team
    {
        Self = 0,
        Opponent = 1,
    }

    public static class TeamExtension
    {
        public static Team Reverse(this Team team)
        {
            return team is Team.Self ? Team.Opponent : Team.Self;
        }
    }
}