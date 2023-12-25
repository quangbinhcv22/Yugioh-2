using System.Collections.Generic;
using System.Linq;
using gameplay.present;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Networks
{
    public static partial class Network
    {
        public static partial class Request
        {
            public static partial class Fighting
            {
                public static void AttackTableCardDirect(Request_AttackTableDirect data)
                {
                    Send(MessageID.ATTACK_TABLE_DIRECT, data);
                }

                public static void AttackTableCard(Request_AttackTableCard data)
                {
                    Send(MessageID.ATTACK_TABLE_CARD, data);
                }
            }
        }

        public static partial class HandleResponse
        {
            public static partial class Fighting
            {
                public static void AttackTableDirect(JObject data)
                {
                    var response = data.ToObject<Response_AttackTableDirect>();

                    var isSelfAttack = Query.IsSelf(response.player);
                    var attackPlayerIndex = isSelfAttack ? 0 : 1;
                    var damagedPlayerIndex = DamagedPlayerIndex(attackPlayerIndex, response.damage);

                    
                    var damage = response.damage;
                    Server_DueManager.main.ModifyHp_ByAttack(damagedPlayerIndex, -Mathf.Abs(response.damage));

                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = attackPlayerIndex,
                        attackerGuid = response.sourceCard.Guid,
                        damagedPlayerIndex = damagedPlayerIndex,
                        damage = damage,
                    });
                }

                private static int DamagedPlayerIndex(int attackPlayerIndex, int damage)
                {
                    if (attackPlayerIndex == 0) return damage >= 0 ? 1 : 0;
                    else return damage >= 0 ? 0 : 1;
                }

                public static void AttackTableCard(JObject data)
                {
                    var response = data.ToObject<Response_AttackTableCard>();

                    var isSelfAttack = Query.IsSelf(response.player);

                    var attackPlayerIndex = isSelfAttack ? 0 : 1;
                    var damagedPlayerIndex = DamagedPlayerIndex(attackPlayerIndex, response.damage);
                    

                    var monsterAttacker = response.sourceCard.Guid;
                    var monsterDefender = response.targetCard.Guid;

                    var defender = DueCardQuery.GetData(monsterDefender);
                    if(string.IsNullOrEmpty(defender.code)) defender.code = response.targetCard.code;

                    
                    Server_DueManager.main.ModifyHp_ByAttack(damagedPlayerIndex, -Mathf.Abs(response.damage));


                    Notifier_DueData.Current.Event_Attack(new Event_Attack()
                    {
                        playerAttackIndex = attackPlayerIndex,
                        attackerGuid = monsterAttacker,
                        defenderGuid = monsterDefender,
                        damagedPlayerIndex = damagedPlayerIndex,
                        damage = response.damage,
                    });
                }
            }
        }
    }

    public struct Request_AttackTableCard
    {
        public long targetCardId;
        public long sourceCardId;
    }

    public struct Response_AttackTableCard
    {
        public long player;

        public ServerCard sourceCard;
        public ServerCard targetCard;

        public int damage;
        public List<DebugPlayer> debugPlayers;

        public List<ServerCard> destroyedCards;

        public struct DebugPlayer
        {
            public int healthPoint;
            public object playerId;
        }
    }


    public class Request_AttackTableDirect
    {
        public long sourceCardId;
    }

    public struct Response_AttackTableDirect
    {
        public long player;
        public int damage;
        public ServerCard sourceCard;
    }
}