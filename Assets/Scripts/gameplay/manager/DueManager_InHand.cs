using System;
using Networks;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        public static Action onSync_InHand;

        public static void Sync_InHand(Response_LoadGameState gameState)
        {
            Manager.self.zone.inHand.Sync(0, gameState.myHandCards);
            Manager.opponent.zone.inHand.SyncAmony(1, gameState.opponentNumberHandCards);
        }
    }
}