using System;
using Networks;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        public static Action onSync_Turn;
        public static Action onSync_Phase;

        public static void SyncTurnAndPhase(Response_LoadGameState gameState)
        {
            Network.Cached.Fighting.round = new Response_StartRound()
            {
                index = 1,
                player = gameState.turnedPlayerId,
                // timeout = 60000,
            };

            Network.Cached.Fighting.turnCount = 1;
            
            SyncPhase(gameState);
        }

        public static void SyncPhase(Response_LoadGameState gameState)
        {
            Network.Cached.Fighting.phase = new Response_OnPhase()
            {
                player = gameState.turnedPlayerId,
                phase = gameState.activePhase,
            };
            
            onSync_Phase?.Invoke();
        }

        public static void OnStartRound()
        {
            var data = Network.Cached.Fighting.round;

            Network.Event.Fighting.startRound?.Invoke(data);
            DueNotifier.Notify_ToTurn(Network.Query.IsSelf(data.player) ? 0 : 1);
        }

        public static void OnStartPhase(Response_OnPhase response)
        {
            Network.Cached.Fighting.phase = response;
            Network.Event.Fighting.onPhase?.Invoke(response);
        }
    }
}