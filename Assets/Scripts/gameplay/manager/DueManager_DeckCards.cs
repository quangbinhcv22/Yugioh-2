using System;
using Networks;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        public static Action onSync_DeckCard;

        public static void Sync_DeckCard(Response_LoadGameState gameState)
        {
            Manager.self.zone.mainDeck.remain = gameState.myRemainNumberDeckCards;
            Manager.opponent.zone.mainDeck.remain = gameState.opponentRemainNumberDeckCards;

            onSync_DeckCard?.Invoke();
        }
    }
}