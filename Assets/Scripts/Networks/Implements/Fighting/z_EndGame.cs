using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QBPlugins.ScreenFlow;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Cached
        {
            public static partial class Fighting
            {
                public static bool isWin;
            }
        }
        
        public static partial class HandleResponse
        {
            public static async void EndGame(JObject data)
            {
                var response = data.ToObject<Response_EndGame>();
                
                var selfHp = response.players.First(p => p.playerId == Cached.matching_self.id);
                var opponentHp = response.players.First(p => p.playerId != Cached.matching_self.id);
                Cached.Fighting.isWin = selfHp.healthPoint > opponentHp.healthPoint;
                
                await UniTask.Delay(3000);

                ScreenManager.Open(ScreenKey.BattleResult);
            }
        }
    }
    
    public struct Response_EndGame
    {
        public List<PlayerResult> players;
        
        public struct PlayerResult
        {
            public long playerId;
            public int healthPoint;
        }
    }
}