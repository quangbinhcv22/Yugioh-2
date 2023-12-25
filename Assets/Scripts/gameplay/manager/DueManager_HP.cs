using gameplay.present;
using Networks;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        public static void Set_MyHp(int hp, Event_Changed_LifePoint.Reason reason)
        {
            Manager.self.hp = hp;

            Notifier_DueData.Current.Changed_lifePoint(new Event_Changed_LifePoint()
            {
                playerIndex = 0,
                reason = reason,
            });
        }

        public static void Set_OpponentHp(int hp, Event_Changed_LifePoint.Reason reason)
        {
            Manager.opponent.hp = hp;

            Notifier_DueData.Current.Changed_lifePoint(new Event_Changed_LifePoint()
            {
                playerIndex = 1,
                reason = reason,
            });
        }

        public static void Reset_Hp()
        {
            Set_MyHp(DueConstant.hp, Event_Changed_LifePoint.Reason.StartGame);
            Set_OpponentHp(DueConstant.hp, Event_Changed_LifePoint.Reason.StartGame);
        }

        public static void Sync_Hp(Response_LoadGameState gameState)
        {
            Set_MyHp(gameState.myHealthPoint, Event_Changed_LifePoint.Reason.Sync);
            Set_OpponentHp(gameState.opponentHealthPoint, Event_Changed_LifePoint.Reason.Sync);
        }
    }
}