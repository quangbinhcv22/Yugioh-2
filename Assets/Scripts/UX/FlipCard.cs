using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UX
{
    public class FlipCard : MonoBehaviour
    {
        [SerializeField] private CanvasGroup front;
        [SerializeField] private CanvasGroup back;

        private Transform _cardTrans;

        [Header("UX")] [SerializeField] private float openDuration;
        [SerializeField] private Ease beginEase = Ease.InQuad;
        [SerializeField] private Ease endEase = Ease.OutQuad;
        
        public bool IsBack => back.alpha > 0;


        private void Awake()
        {
            _cardTrans = transform;
        }




        [Button]
        public void Open(bool frontFace = true)
        {
            var duration = openDuration / 2f;
            
            _cardTrans.DOLocalRotate(new Vector3(0,90f,45f), duration).SetEase(beginEase).onComplete += () =>
            {
                front.alpha = frontFace ? 0 : 1;
                back.alpha = frontFace ? 1 : 0;
                
                _cardTrans.DOLocalRotate(new Vector3(0, 0f, 0f), duration).SetEase(endEase);
            };
        }
        
        [Button]
        public void Close(bool frontFace = false)
        {
            var duration = openDuration / 2f;
            
            _cardTrans.DOLocalRotate(new Vector3(0,-90f,45f), duration).SetEase(beginEase).onComplete += () =>
            {
                front.alpha = frontFace ? 0 : 1;
                back.alpha = frontFace ? 1 : 0;
                
                _cardTrans.DOLocalRotate(new Vector3(0, 0f, 90f), duration).SetEase(endEase);
            };
        }
        
        
        public void ShowFront()
        {
            // _cardTrans.localEulerAngles = new Vector3(0, 0, 0);
            front.alpha = 1;
            back.alpha = 0;
        }
        
        public void ShowBack()
        {
            // _cardTrans.localEulerAngles = new Vector3(0, 0, 0);
            front.alpha = 0;
            back.alpha = 1;
        }
    }
}