using System;
using battle.define;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gameplay.present
{
    [DefaultExecutionOrder(int.MinValue + 1)]
    public class PresentHandler_LifePoint : Singleton<PresentHandler_LifePoint>
    {
        [SerializeField] private float blendDuration = 1f;
        [SerializeField] private Ease blendEase = Ease.InOutQuad;


        [ShowInInspector, HideInEditorMode] private BlendableNumber _selfLp = new();
        [ShowInInspector, HideInEditorMode] private BlendableNumber _opponentLp = new();

        public int GetValue(Team team) => (int)(team is Team.Self ? _selfLp.value : _opponentLp.value);


        public Action<Team> onChanged;
        public Action<Team> onEnd;


        protected override void OnEnable()
        {
            base.OnEnable();

            Notifier_DueData.Current.changed_lifePoint += OnChanged_LifePoint;
        }

        private void OnDisable()
        {
            Notifier_DueData.Current.changed_lifePoint -= OnChanged_LifePoint;
        }


        public void OnChanged_LifePoint(Event_Changed_LifePoint @event)
        {
            var teamChanged = Client_DueManager.IsSelf(@event.playerIndex) ? Team.Self : Team.Opponent;
            
            switch (@event.reason)
            {
                case Event_Changed_LifePoint.Reason.StartGame:
                case Event_Changed_LifePoint.Reason.Sync:
                    UpdateImmediately(teamChanged);
                    break;
                case Event_Changed_LifePoint.Reason.NewRound:
                    break;
                case Event_Changed_LifePoint.Reason.Attack:
                    UpdateBlend(teamChanged);
                    break;
            }
        }


        private void FetchData(Team team)
        {
            if (team is Team.Self) _selfLp.value = GetData(Team.Self);
            else _opponentLp.value = GetData(Team.Opponent);
        }

        private int GetData(Team team)
        {
            return Client_DueManager.GetPlayer(team).hp;
        }


        private void UpdateImmediately(Team team)
        {
            FetchData(team);
            onChanged?.Invoke(team);
        }


        private void UpdateBlend(Team team)
        {
            var newValue = GetData(team);
            var blendNumber = team is Team.Self ? _selfLp : _opponentLp;

            blendNumber.DOBlend(newValue, blendDuration).SetEase(blendEase).onUpdate += () =>
            {
                onChanged?.Invoke(team);
            };
        }
    }

    public struct Event_Changed_LifePoint
    {
        public enum Reason
        {
            StartGame = 1,
            NewRound = 2,
            Attack = 3,
            Sync = 4,
        }

        public int playerIndex;
        public Reason reason;
    }
}