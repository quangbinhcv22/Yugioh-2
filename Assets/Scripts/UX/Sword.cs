using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using event_name;
using Sirenix.OdinInspector;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

namespace UX
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private float rotateDuration = 0.15f;
        [SerializeField] private Ease rotateEase = Ease.OutQuad;
        [SerializeField] private float toDuration = 1f;
        [SerializeField] private float delay = 0.25f;
        [SerializeField] private Ease toEase = Ease.OutQuad;
        
        
        public void Highlight(bool highlight)
        {
            image.color = highlight ? Color.white : new Color(1, 1, 1, 0.5f);
        }


        public Tween Fly(Vector3 from, Vector3 to)
        {
            gameObject.SetActive(true);

            transform.position = from;
            transform.eulerAngles = from.y < to.y ? Vector3.zero : new Vector3(0,0,180);


            transform.DORotate(AnglesTo(to), rotateDuration).SetEase(rotateEase);

            var tween = transform.DOMove(to, toDuration).SetEase(toEase);
            tween.onComplete += OnFlyDone;

            return tween;
        }

        private async void OnFlyDone()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            gameObject.SetActive(false);
        }


        private Vector3 AnglesTo(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return new Vector3(0, 0, angle - 90);
        }
    }
}