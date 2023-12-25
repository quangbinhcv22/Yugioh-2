using System;
using Networks;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        public static Action onSync_FieldCard;

        public static void Sync_FieldCard(Response_LoadGameState gameState)
        {
            Manager.self.zone.field.Sync(0, gameState.myFieldCard);
            Manager.opponent.zone.field.Sync(1, gameState.opponentFieldCard);
        }
    }
}