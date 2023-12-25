using System;
using DG.Tweening;
using Gameplay.card.ui;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UX
{
    public class FlipCard : MonoBehaviour
    {
        [SerializeField] private CanvasGroup front;
        [SerializeField] private CanvasGroup back;
        [SerializeField] private UI_Card card;

        private Transform _cardTrans;

        [Header("UX")] [SerializeField] private float openDuration;
        [SerializeField] private Ease beginEase = Ease.InQuad;
        [SerializeField] private Ease endEase = Ease.OutQuad;

        public bool IsBack => back.alpha > 0;


        private void Awake()
        {
            _cardTrans = transform;
        }


        public void SetVertical()
        {
            _cardTrans.eulerAngles = Vector3.zero;
        }

        public void SetHorizontal()
        {
            _cardTrans.eulerAngles = new Vector3(0, 0f, 90f);
        }


        [Button]
        public void Open(bool frontFace = true)
        {
            var duration = openDuration / 2f;

            _cardTrans.DOLocalRotate(new Vector3(0, 90f, 45f), duration).SetEase(beginEase).onComplete += () =>
            {
                front.alpha = frontFace ? 0 : 1;
                back.alpha = frontFace ? 1 : 0;

                CallbackRefresh();

                _cardTrans.DOLocalRotate(new Vector3(0, 0f, 0f), duration).SetEase(endEase);
            };
        }

        [Button]
        public void Close(bool frontFace = false)
        {
            var duration = openDuration / 2f;

            _cardTrans.DOLocalRotate(new Vector3(0, -90f, 45f), duration).SetEase(beginEase).onComplete += () =>
            {
                front.alpha = frontFace ? 0 : 1;
                back.alpha = frontFace ? 1 : 0;

                CallbackRefresh();

                _cardTrans.DOLocalRotate(new Vector3(0, 0f, 90f), duration).SetEase(endEase);
            };
        }


        public void ShowFront()
        {
            // _cardTrans.localEulerAngles = new Vector3(0, 0, 0);
            front.alpha = 1;
            back.alpha = 0;

            CallbackRefresh();
        }

        public void ShowBack()
        {
            // _cardTrans.localEulerAngles = new Vector3(0, 0, 0);
            front.alpha = 0;
            back.alpha = 1;

            CallbackRefresh();
        }

        private void CallbackRefresh()
        {
            card ??= GetComponentInParent<UI_Card>();
            card.HandleChangePosition_OnBack();
        }
    }
}