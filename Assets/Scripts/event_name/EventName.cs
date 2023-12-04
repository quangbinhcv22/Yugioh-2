using battle.define;
using Gameplay;

namespace event_name
{
    public static class EventName
    {
        public static class Gameplay
        {
            public const string StartGame = "StartGame";
            public const string DrawDefault = "DrawDefault";
            public const string Draw = "Draw";
            public const string ToGraveyard = "ToGraveyard";

            // public const string ZoneChanged_MainMonster = "ZoneChanged_MainMonster";
            public const string ZoneChanged_InHand = "ZoneChanged_InHand";

            // public const string Summon = "Summon";

            public const string ToTurn = "ToTurn";
            // public const string ToPhase = "ToPhase";
            public const string OutTime = "OutTime";

            public const string UI_RefreshPhase = "UI_RefreshPhase";

            public const string MonsterDie = "MonsterDie";
            

            public static class Data
            {
                public const string UpdateRound = "Data_UpdateRound";
            }

            public static class Present
            {
                public const string UpdateRound = "Present_UpdateRound";

                public const string StartAttack = "Present_StartAttack";
                public const string EndAttack = "Present_EndAttack";
            }
        }

        public const string SelectCard = "SelectCard";
    }
}


public class Event_ToTurn
{
    public int playerIndex;
}

public class Event_ToPhase
{
    public Phase phase;
}

