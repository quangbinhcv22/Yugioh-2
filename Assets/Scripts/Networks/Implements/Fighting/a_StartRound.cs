using System;
using System.Collections.Generic;
using battle.define;
using Cysharp.Threading.Tasks;
using gameplay.manager;
using Newtonsoft.Json.Linq;
using QBPlugins.ScreenFlow;
using Random = UnityEngine.Random;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Event
        {
            public static partial class Fighting
            {
                public static Action<Response_StartRound> startRound;
                public static Action<Response_OnPhase> onPhase;
            }
        }


        public static partial class Cached
        {
            public static partial class Fighting
            {
                public static int turnCount;
                public static Response_StartGame startGame;
                public static Response_StartRound round;
                public static Response_OnPhase phase;
                public static DateTime endRoundTime;
            }
        }

        public static partial class Request
        {
            public static partial class Fighting
            {
                public static void SwitchPhase(Phase phase)
                {
                    switch (phase)
                    {
                        case Phase.Battle:
                            ACTIVE_BATTLE_PHASE();
                            break;
                        case Phase.Main2:
                            break;
                        case Phase.End:
                            EndPhase();
                            break;
                    }
                }


                private static void ACTIVE_BATTLE_PHASE()
                {
                    Send(MessageID.ACTIVE_BATTLE_PHASE);
                }

                private static void EndPhase()
                {
                    Send(MessageID.END_PHASE);
                }
            }
        }

        public static partial class HandleResponse
        {
            public static async void BattleStartGame(JObject data)
            {
                var response = data.ToObject<Response_StartGame>();

                Cached.Fighting.startGame = response;
                Cached.Fighting.turnCount = -1;
                Cached.Fighting.revealedCards.Clear();

                await UniTask.Delay(1750);
                ScreenManager.Open(ScreenKey.LoadingToBattle);
            }

            public static async void StartRound(JObject data)
            {
                var response = data.ToObject<Response_StartRound>();
                Cached.Fighting.endRoundTime = DateTime.Now + TimeSpan.FromMilliseconds(response.timeout);
                
                
                await UniTask.WaitUntil(() => Server_DueManager.main);

                Cached.Fighting.round = response;
                Cached.Fighting.turnCount++;

                
                DueManager.OnStartRound();

                Server_DueManager.main.self.normalSummonThisTurn = false;
            }

            public static void OnPhase(JObject data)
            {
                var response = data.ToObject<Response_OnPhase>();
                DueManager.OnStartPhase(response);
            }
        }
    }

    public struct Response_StartGame
    {
        public List<ServerCard> cards;
    }

    public struct Response_StartRound
    {
        public int index;
        public int timeout;
        public long player;
    }

    public class Response_OnPhase
    {
        public string phase;
        public bool existActivableCards;
        public long player;
    }

    [Serializable]
    public class ServerCard
    {
        public static ServerCard NewAnonymous()
        {
            var result = new ServerCard
            {
                id = Random.Range(int.MinValue, -1),
            };

            return result;
        }

        public static List<ServerCard> New5Anonymous()
        {
            var cards = new List<ServerCard>();
            for (int i = 0; i < 5; i++)
            {
                cards.Add(NewAnonymous());
            }

            return cards;
        }

        public long id;
        public string code;

        public int atk;
        public int def;
        public string position;

        public string Guid
        {
            get => id.ToString();
            set => id = long.Parse(value);
        }
    }
}