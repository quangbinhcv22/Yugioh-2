using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Event
        {
            public static partial class Fighting
            {
                public static Action<Response_DrawCheckCard> selfDrawCard;
                public static Action<Response_DrawCheckCard> opponentDrawCard;
            }
        }

        public static partial class HandleResponse
        {
            public static void DrawCheckCard(JObject data)
            {
                var response = data.ToObject<Response_DrawCheckCard>();

                if (Query.IsSelf(response.PlayerID))
                {
                    Event.Fighting.selfDrawCard?.Invoke(response);
                    Server_DueManager.main.Draw(0, new List<ServerCard>(){response.card});
                }
                else
                {
                    Event.Fighting.opponentDrawCard?.Invoke(response);
                    Server_DueManager.main.Draw(1, new(){ServerCard.NewAnonymous()});
                }
            }
        }

        public struct Response_DrawCheckCard
        {
            public long player;
            public ServerCard card;

            public string PlayerID => player.ToString();
        }
    }
}