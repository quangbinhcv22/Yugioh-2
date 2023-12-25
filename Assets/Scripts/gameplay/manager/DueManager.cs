using Cysharp.Threading.Tasks;
using Gameplay.card;
using Networks;
using QBPlugins.ScreenFlow;

namespace gameplay.manager
{
    public static partial class DueManager
    {
        static Server_DueManager Manager => Server_DueManager.main;


        public static void Sync_GameState(Response_LoadGameState gameState)
        {
            if (Network.Cached.loadGameStateReason is Network.Request.LoadGameStateReason.Reconnect)
            {
                Sync_Hp(gameState);
                Sync_DeckCard(gameState);
                SyncTurnAndPhase(gameState);
            
                Sync_InHand(gameState);
                Sync_TableCard(gameState);
                
                Sync_FieldCard(gameState);
            }
            
            State_Stats.Sync(gameState);
        }


        public static bool isReloadingBattle;
        public static Response_LoadGameState cachedGameState;

        public static async void ReloadBattle(Response_LoadGameState gameState)
        {
            isReloadingBattle = true;
            cachedGameState = gameState;

            await UniTask.Delay(500);
            ScreenManager.Open(ScreenKey.LoadingToBattle);
        }

        public static void StartSync_ByReloading()
        {
            Sync_GameState(cachedGameState);
            isReloadingBattle = false;
        }
    }
}