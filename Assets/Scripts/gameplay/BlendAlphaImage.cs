using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlendAlphaImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.OutQuad;

    private Tween _tween;
    
    private void OnEnable()
    {
        image.DOFade(0f, 0f);
        _tween = image.DOFade(1f, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }
    
    private void OnDisable()
    {
        _tween?.Kill();
    }
}