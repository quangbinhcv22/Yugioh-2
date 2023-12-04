using System;
using DG.Tweening;
using Gameplay.board;
using Sirenix.OdinInspector;
using UnityEngine;

namespace battle.view.card
{
    public class View_CardDirection : MonoBehaviour
    {
        [SerializeField] private float openDuration = 0.35f;
        [SerializeField] private Ease beginEase = Ease.InQuad;
        [SerializeField] private Ease endEase = Ease.OutQuad;


        private Action onDone;


        private MonsterPosition _currentPosition = MonsterPosition.Unset;

        [Button]
        public void SetPosition(MonsterPosition position)
        {
            if (_currentPosition == position) return;
            _currentPosition = position;

            switch (position)
            {
                case MonsterPosition.Unset:
                    SwitchVertical();
                    break;
                case MonsterPosition.Standard:
                    SwitchVertical();
                    break;
                case MonsterPosition.Attack:
                    Tween_FlipVertical();
                    break;
                case MonsterPosition.Defense:
                    Tween_FlipHorizontal();
                    break;
            }
        }


        [Button]
        public void SwitchVertical()
        {
            transform.eulerAngles = Vector3.zero;
            onDone?.Invoke();
        }


        [Button]
        public void Tween_FlipVertical()
        {
            var duration = openDuration / 2f;

            transform.DOLocalRotate(new Vector3(0, 90f, 45f), duration).SetEase(beginEase).onComplete += () =>
            {
                onDone?.Invoke();
                transform.DOLocalRotate(new Vector3(0, 0f, 0f), duration).SetEase(endEase);
            };
        }

        [Button]
        public void Tween_FlipHorizontal()
        {
            var duration = openDuration / 2f;

            transform.DOLocalRotate(new Vector3(0, -90f, 45f), duration).SetEase(beginEase).onComplete += () =>
            {
                onDone?.Invoke();
                transform.DOLocalRotate(new Vector3(0, 0f, 90f), duration).SetEase(endEase);
            };
        }
    }
}