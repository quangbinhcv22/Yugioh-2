using System;
using System.Collections.Generic;
using System.Linq;
using battle.define;
using Cysharp.Threading.Tasks;
using Gameplay.board;
using UnityEngine;
using UX;

namespace gameplay.present
{
    [DefaultExecutionOrder(int.MinValue + 1)]
    public class PresentHandler_Attack : Singleton<PresentHandler_Attack>
    {
        public bool IsPresenting;
        public Action presentCompleted;
        
        [SerializeField] private Sword sword;
        [SerializeField] private Transform self_AttackedPoint;
        [SerializeField] private Transform opponent_AttackedPoint;


        protected override void OnEnable()
        {
            base.OnEnable();

            Notifier_DueData.Current.event_attack += OnEvent_Attack;
        }

        private void OnDisable()
        {
            Notifier_DueData.Current.event_attack -= OnEvent_Attack;
        }


        //check bộ chọn tấn công
        private void OnEvent_Attack(Event_Attack @event)
        {
            IsPresenting = true;

            if (Networks.Network.Query.Fighting.IsOwnTurn)
            {
                CardAction_PhaseBattle.Current.attackedGuids.Add(@event.attackerGuid);
            }
            
            
            var attackTeam = Client_DueManager.GetTeam(@event.playerAttackIndex);
            var defenderTeam = Client_DueManager.GetRemainingTeam(@event.playerAttackIndex);
            var damagedTeam = Client_DueManager.GetTeam(@event.damagedPlayerIndex);
            var damage = Mathf.Abs(@event.damage);

            var fromPoint = GetCardPoint(@event.attackerGuid);
            var toPoint = @event.IsAttackDirect ? GetMainPoint(defenderTeam) : GetCardPoint(@event.defenderGuid);

            sword.Fly(fromPoint, toPoint).onComplete += OnDone_SwordFly;

            async void OnDone_SwordFly()
            {
                // mở bài thủ
                if (!@event.IsAttackDirect)
                {
                    var defenderSpace = DueCardQuery.GetUICard_CombatSpace(@event.defenderGuid);
                    if (defenderSpace.MonsterPosition is MonsterPosition.Defense)
                    {
                        defenderSpace.OnAttacked_OpenIfCan();
                        await UniTask.Delay(750);
                    }
                }


                UI_DamageText.Current.Show(damagedTeam == attackTeam ? fromPoint : toPoint, damage);


                PresentHandler_LifePoint.Current.OnChanged_LifePoint(new Event_Changed_LifePoint
                {
                    playerIndex = @event.damagedPlayerIndex,
                    reason = Event_Changed_LifePoint.Reason.Attack
                });


                var dieGuids = @event.dieCards;
                if (dieGuids != null && dieGuids.Any())
                {
                    foreach (var dieGuid in dieGuids)
                    {
                        var spaceSlot = DueCardQuery.GetUICard_CombatSpace(dieGuid);
                        spaceSlot.Die();
                    }
                }

                IsPresenting = false;
                presentCompleted?.Invoke();
            }
        }


        private Vector3 GetMainPoint(Team team)
        {
            return team is Team.Self ? self_AttackedPoint.position : opponent_AttackedPoint.position;
        }

        private Vector3 GetCardPoint(string cardGuid)
        {
            return DueCardQuery.GetUICard(cardGuid).transform.position;
        }
    }

    public struct Event_Attack
    {
        public int playerAttackIndex;

        public string attackerGuid;
        public string defenderGuid;

        public int damagedPlayerIndex;
        public int damage;

        public List<string> dieCards;

        public bool IsAttackDirect => string.IsNullOrEmpty(defenderGuid);
    }
}