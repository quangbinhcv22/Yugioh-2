using Gameplay.board;
using Newtonsoft.Json.Linq;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Request
        {
            public static partial class Fighting
            {
                public static void ReleaseHandCard(Request_ReleaseHandCard data)
                {
                    Send(MessageID.RELEASE_HAND_CARD, data);


                    // var fakeResponse = new Response_ReleaseHandCard()
                    // {
                    //     player = Cached.playerInfo.id,
                    //     cardCode = data.cardCode,
                    //     releaseType = data.releaseType,
                    // };
                    //
                    // SimulateMessage(fakeResponse);
                }
            }
        }

        public static partial class HandleResponse
        {
            public static partial class Fighting
            {
                public static void ReleaseHandCard(JObject data)
                {
                    var response = data.ToObject<Response_ReleaseHandCard>();

                    var isSelf = Query.IsSelf(response.player);
                    var player = isSelf ? Server_DueManager.main.player1 : Server_DueManager.main.player2;
                    var playerIndex = isSelf ? 0 : 1;
                    var position = response.releaseType == "SUMMON" ? MonsterPosition.Attack : MonsterPosition.Defense;

                    player.normalSummonThisTurn = true;
                    var card = player.zone.inHand.Release(response.card.Guid, response.card);
                    var index = position is MonsterPosition.Attack ? player.zone.mainMonster.Summon_Attack(card) : player.zone.mainMonster.Summon_Defense(card);
                    
                    DueNotifier.Notify_InHandRemove(playerIndex, card.Guid);

                    Notifier_DueData.Current.Event_SummonMonster(new()
                    {
                        playerIndex = playerIndex,
                        summonGuid = card.Guid,
                        position = position,
                        summonIndex = index,
                    });
                }
            }
        }
    }

    public struct Request_ReleaseHandCard
    {
        public string releaseType;
        public long cardId;
    }

    public class Response_ReleaseHandCard
    {
        public string releaseType;
        public long player;
        public ServerCard card;
    }
}