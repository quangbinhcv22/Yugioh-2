
using System;
using battle.define;
using event_name;
using Networks;
using TigerForge;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        public static Action onSync_TableCard;

        public static void Sync_TableCard(Response_LoadGameState gameState)
        {
            Manager.self.zone.mainMonster.Sync(0, gameState.myTableCards);
            Manager.opponent.zone.mainMonster.Sync(1, gameState.opponentTableCards);
            
            onSync_TableCard?.Invoke();
            
            EventManager.EmitEvent(EventName.Gameplay.UI_RefreshPhase);
        }

        public static CardSpace_Combat GetMonsterSpace(Team team, int index)
        {
            var player = team is Team.Self ? Manager.self : Manager.opponent;
            return player.zone.mainMonster.Get(index);
        }
    }
}