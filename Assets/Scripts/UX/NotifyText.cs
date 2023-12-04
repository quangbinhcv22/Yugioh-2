using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace UX
{
    public class NotifyText : MonoBehaviour
    {
        [SerializeField] private TMP_Text contentText;
        [SerializeField] private RectTransform contentTrans;
        [SerializeField] private RectTransform enterPoint;
        [SerializeField] private RectTransform exitPoint;
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private float delay = 1f;
        [SerializeField] private Ease ease;

        public static NotifyText main;

        private void OnEnable()
        {
            main = this;
        }
        
        

        [Button]
        public static void Notify(string text)
        {
            if(main == null) return;
            
            main.contentText.SetText(text);
            main.Enter();
        }
        
        public static void SetColor(Color color)
        {
            if(main == null) return;

            main.contentText.color = color;
        }


        public void Enter()
        {
            contentTrans.localPosition = enterPoint.localPosition;
            
            contentTrans.DOLocalMove(Vector3.zero, duration).SetEase(ease).onComplete += () =>
            {
                Invoke(nameof(Exit), delay);
            };
        }

        public void Exit()
        {
            contentTrans.DOLocalMove(exitPoint.localPosition, duration).SetEase(ease);
        }
    }
}